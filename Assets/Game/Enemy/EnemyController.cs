using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Enemy {

	public enum ReactiveAbility {
		Block,
		StoneBoots,
		Dodge,
		Countershot,
		MagicImmunity,
		DodgeCharge
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
			m_ReactiveAbilities.Add(ReactiveAbility.Countershot);
			m_ReactiveAbilities.Add(ReactiveAbility.MagicImmunity);
			m_ReactiveAbilities.Add(ReactiveAbility.DodgeCharge);
			
			
			ApproachTask.InitTask(this, m_PlayerCharacter, null);
			
			
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

		public void Notify(PlayerController player, AttackBase attackPrefab) {
			//notify
		}
		
	}
	
}