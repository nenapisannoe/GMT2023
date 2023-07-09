using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.PlayerAttacks;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Game.Enemy {

	public enum ReactiveAbility {
		Block,
		StoneBoots,
		Dodge,
		MagicImmunity,
		DodgeCharge,
		AvoidWater,
		AvoidMagma
	}
	
	public class EnemyController : Character {

		[SerializeField] private Character m_PlayerCharacter;
		
		[Space]
		[Header("Attacks")]
		[SerializeField] private AttackBase MeleeAttackPrefab;
		[SerializeField] private AttackBase RangeAttackPrefab;
		[SerializeField] private AttackBase AreaAttackPrefab;

		private List<ReactiveAbility> m_ReactiveAbilities = new List<ReactiveAbility>();
		private List<BaseTask> m_AvailableTasks = new List<BaseTask>();
		private BaseTask ActiveTask;
		private ApproachTask ApproachTask = new ApproachTask();
		
		private float moveSpeed = 2f;

		private void Awake() {
			Init();
		}

		public void Init() {
			m_ReactiveAbilities.Add(ReactiveAbility.Block);
			m_ReactiveAbilities.Add(ReactiveAbility.StoneBoots);
			m_ReactiveAbilities.Add(ReactiveAbility.Dodge);
			m_ReactiveAbilities.Add(ReactiveAbility.MagicImmunity);
			m_ReactiveAbilities.Add(ReactiveAbility.DodgeCharge);

			if (m_ReactiveAbilities.Contains(ReactiveAbility.Block)) {
				isPhysicalImmune = true;
			}
			if (m_ReactiveAbilities.Contains(ReactiveAbility.StoneBoots)) {
				isKnockable = false;
			}
			if (m_ReactiveAbilities.Contains(ReactiveAbility.MagicImmunity)) {
				isMagicImmune = true;
			}
			
			
			ApproachTask.InitTask(this, m_PlayerCharacter, null);
			BaseTask open_chest_task = new ChestOpeningTask();
			open_chest_task.InitTask(this, null, null);
			m_AvailableTasks.Add(open_chest_task);
			
			
			BaseTask task = new MeleeAttackTask();
			task.InitTask(this, m_PlayerCharacter, MeleeAttackPrefab);
			m_AvailableTasks.Add(task);
			/*
			task = new RangeAttackTask();
			task.InitTask(this, m_PlayerCharacter, RangeAttackPrefab);
			m_AvailableTasks.Add(task);
			task = new AreaAttackTask();
			task.InitTask(this, m_PlayerCharacter, AreaAttackPrefab);
			m_AvailableTasks.Add(task);
			*/
		}

		private void Update() {
			if (actionsLocked) {
				return;
			}
			if (isStunned) {
				return;
			}
			EnemyTick();
		}

		private void EnemyTick() {
			if (ActiveTask == null) {
				foreach (var task in m_AvailableTasks) {
					if (task.CanExecuteTask()) {
						ActiveTask = task;
						break;
					}
				}
			}

			if (ActiveTask == null) {
				if (ApproachTask.CanExecuteTask()) {
					RunTask(ApproachTask);
				}
				else {
					SetVelocity(Vector2.zero);
				}
				return;
			}
			
			RunTask(ActiveTask);
		}

		private void RunTask(BaseTask task) {
			var et = task.GetTaskExecutor();
			switch (et) {
				case ExecutorTask.MoveToPosition:
					RunMoveToPositionTask(task.GetMoveToPositionTaskData());
					break;
				
				case ExecutorTask.AttackTarget:
					task.RunTask();
					RunAttackTargetTask(task, task.GetAttackTargetTaskData());
					task.CompleteTask();
					ActiveTask = null;
					break;
				
				case ExecutorTask.OpenChest:
					task.RunTask();
					RunOpenChestTask(task);
					task.CompleteTask();
					ActiveTask = null;
					break;
				
				default:
					throw new ArgumentOutOfRangeException(nameof(et), et, null);
				
			}
		}

		private void RunMoveToPositionTask(Vector3 target) {
			var dir = target - transform.position;
			var vel = (Vector2)(dir.normalized * moveSpeed);
			SetVelocity(vel);
		}

		private void RunAttackTargetTask(BaseTask task, Vector3 target) {
			SetVelocity(Vector2.zero);
			MakeBasicAttack(task.AttackPrefab, target);
		}
		
		private async void RunOpenChestTask(BaseTask task) {
			SetVelocity(Vector2.zero);
			if (task.target is ChestContriller target_chest) {
				//Здесь нужно вставить начало анимации открытия
				await UniTask.Delay(TimeSpan.FromSeconds(2f));
				//Здесь нужно вставить конец анимации открытия

				if (target_chest.isDanger){
					isStunned = true;
					await UniTask.Delay(TimeSpan.FromSeconds(2f));
					isStunned = false;
				}else{
					currentHealth += 10;
				}
				target_chest.open();
				ApproachTask.InitTask(this, m_PlayerCharacter, null);
			}

		}

		private bool isImmune;
		private bool isPhysicalImmune;
		private bool isMagicImmune;
		
		public override void Hit(Damage damage) {
			if (isPhysicalImmune && damage.Type == DamageType.Physical) {
				return;
			}
			if (isMagicImmune && damage.Type == DamageType.Magic) {
				return;
			}
			base.Hit(damage);
			if ((ChestsStorage.active_chests.Count > 0) && (currentHealth <= 50)){
				ApproachTask.InitTask(this, ChestsStorage.active_chests[0], null);
			}
		}

		public async void Notify(PlayerController player, AttackBase attackPrefab) {
			if (attackPrefab is AreaAttack) {
				if (m_ReactiveAbilities.Contains(ReactiveAbility.Dodge)) {
					var dist = BaseTask.GetDistance(transform.position, player.transform.position);
					if (dist < 3.5d) {
						MakeDodge(player.transform);
					}
				}
				return;
			}
			if (attackPrefab is ChargeAttack charge) {
				if (m_ReactiveAbilities.Contains(ReactiveAbility.DodgeCharge)) {
					var dist = BaseTask.GetDistance(transform.position, player.transform.position);
					if (dist < 5d) {
						MakeChargeDodge(charge, player.transform);
					}
				}
				return;
			}
		}

		private async UniTask MakeImmune(int duration) {
			isImmune = true;
			await UniTask.Delay(TimeSpan.FromSeconds(duration));
			isImmune = false;
		}
		
		public async void MakeDodge(Transform from) {
			forceVector = (transform.position - from.position).normalized * 10f;
			await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
			forceVector = Vector2.zero;
		}

		public async void MakeChargeDodge(ChargeAttack attack, Transform from) {
			forceVector = Vector2.Perpendicular(attack.CachePos) * 10f;
			if (transform.position.y > from.position.y) {
				forceVector.y = Math.Abs(forceVector.y);
			}
			else {
				forceVector.y = -Math.Abs(forceVector.y);
			}
			if (transform.position.x > from.position.x) {
				forceVector.x = Math.Abs(forceVector.x);
			}
			else {
				forceVector.x = -Math.Abs(forceVector.x);
			}
			await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
			forceVector = Vector2.zero;
			forceVector = Vector2.Perpendicular(forceVector);
		}
		
	}
	
}