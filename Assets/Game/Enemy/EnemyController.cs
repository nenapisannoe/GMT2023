using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.PlayerAttacks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

namespace Game.Enemy {

	public enum ReactiveAbility {
		Block,
		StoneBoots,
		Dodge,
		MagicImmunity,
		DodgeCharge,
		AvoidWater,
		ImmunityMagma,
		ShootBarrels
	}
	
	public class EnemyController : Character {

		[SerializeField] private Character m_PlayerCharacter;
		
		[Space]
		[Header("Attacks")]
		[SerializeField] private AttackBase MeleeAttackPrefab;
		[SerializeField] private AttackBase RangeAttackPrefab;

		public int SpecialBlockDamage = 15;
		[SerializeField] private GameObject SpecialOnBlockPrefab;
		[SerializeField] private GameObject SpecialMagicImmunePrefab;
		[SerializeField] private GameObject SpecialRegenPrefab;

		private bool rangeAttackEnabled = false;
		private bool regenEnabled = false;
		private bool chestOpeningEnabled = false;
		
		public List<ReactiveAbility> m_ReactiveAbilities = new List<ReactiveAbility>();
		private List<BaseTask> m_AvailableTasks = new List<BaseTask>();
		private BaseTask ActiveTask;
		private ApproachTask ApproachTask = new ApproachTask();
		
		private float moveSpeed = 1.3f;
		
		private List<Damage> attackHistory = new List<Damage>();

		public GameObject sutunEffet;
		
		public int roundCount = 0;

		private bool regenActive;
		private bool regenInterrupted;

		public bool AvoidsWater;

		private void Awake() {
			base.Awake();
			sutunEffet.SetActive(false);
			Init();
		}

		public void OnEnable(){
			
			base.OnEnable();
			sutunEffet.SetActive(false);
			ApproachTask.InitTask(this, m_PlayerCharacter, null);
			BaseTask task = new MeleeAttackTask();
			task.InitTask(this, m_PlayerCharacter, MeleeAttackPrefab);
			m_AvailableTasks.Add(task);
        	Init();           
    	}

		public void Init() {
			if (m_ReactiveAbilities.Contains(ReactiveAbility.Block)) {
				IsBossMeleeAttackImmune = true;
			}
			if (m_ReactiveAbilities.Contains(ReactiveAbility.StoneBoots)) {
				isKnockable = false;
			}
			if (m_ReactiveAbilities.Contains(ReactiveAbility.MagicImmunity)) {
				isAbility2Immune = true;
			}
			if (m_ReactiveAbilities.Contains(ReactiveAbility.AvoidWater)) {
				AvoidsWater = true;
			}
			if (m_ReactiveAbilities.Contains(ReactiveAbility.ImmunityMagma)) {
				isBossMagmaImmune = true;
			}

		}

		private void Update() {
			if (actionsLocked || regenActive) {
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
				
				case ExecutorTask.Regen:
					task.RunTask();
					RunRegenTask(task as RegenTask);
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
					sutunEffet.SetActive(true);
					await UniTask.Delay(TimeSpan.FromSeconds(2f));
					sutunEffet.SetActive(false);
					isStunned = false;
				}else{
					Heal(40);
				}
				target_chest.open();
				ApproachTask.InitTask(this, m_PlayerCharacter, null);
			}

		}

		private async void RunRegenTask(RegenTask task) {
			SetVelocity(Vector2.zero);
			if (actionsLocked) {
				return;
			}
			regenActive = true;
			regenInterrupted = false;
			PlaySpecial(SpecialRegenPrefab, transform, task.RegenDuration);
			var ticks = task.RegenDuration;
			while (!regenInterrupted && currentHealth > 0 && ticks > 0) {
				Heal(task.RegenPower);
				ticks--;
				await UniTask.Delay(TimeSpan.FromSeconds(1));
			}
			regenActive = false;
		}

		private bool isImmune;
		private bool IsBossMeleeAttackImmune;
		private bool isAbility2Immune;
		private bool isBossMagmaImmune;
		
		public override void Hit(Damage damage) {
			if (forceVector != Vector2.zero) {
				return;
			}
			if (IsBossMeleeAttackImmune && damage.Type == DamageType.BossMeleeAttack) {
				return;
			}
			if (isAbility2Immune && damage.Type == DamageType.BossAbility2) {
				return;
			}
			if (isBossMagmaImmune && damage.Type == DamageType.Magma) {
				return;
			}
			base.Hit(damage);
			attackHistory.Add(damage);
			if (chestOpeningEnabled && (ChestsStorage.active_chests.Count > 0) && (currentHealth <= 50)){
				ApproachTask.InitTask(this, ChestsStorage.active_chests[0], null);
			}
		}

		public async void Notify(PlayerController player, AttackBase attackPrefab) {
			if (attackPrefab is AreaAttack && !isInWater) {
				if (m_ReactiveAbilities.Contains(ReactiveAbility.Dodge)) {
					var dist = BaseTask.GetDistance(transform.position, player.transform.position);
					if (dist < 3.5d) {
						MakeDodge(player.transform);
					}
				}
				return;
			}
			if (attackPrefab is ChargeAttack charge && !isInWater) {
				if (m_ReactiveAbilities.Contains(ReactiveAbility.DodgeCharge)) {
					var dist = BaseTask.GetDistance(transform.position, player.transform.position);
					if (dist < 5d) {
						MakeChargeDodge(charge, player.transform);
					}
				}
				return;
			}
		}

		public async void HitNotify(Character player, AttackBase attackPrefab) {
			if (attackPrefab is BossMeleeAttack && IsBossMeleeAttackImmune) {
				PlaySpecial(SpecialOnBlockPrefab, player.transform);
				player.Hit(new Damage {
					Attacker = this,
					Type = DamageType.HeroAbility1,
					Value = SpecialBlockDamage
				});
				return;
			}
			if (attackPrefab is ProjectileAttack && isAbility2Immune) {
				PlaySpecial(SpecialMagicImmunePrefab, transform);
			}
			
			if (!regenInterrupted && attackPrefab is ChannelingAttack) {
				regenInterrupted = true;
			}
		}

		private async UniTask MakeImmune(int duration) {
			isImmune = true;
			await UniTask.Delay(TimeSpan.FromSeconds(duration));
			isImmune = false;
		}

		private async void PlaySpecial(GameObject prefab, Transform target, int duration = 4) {
			var instance = Instantiate(prefab, target, false);
			await UniTask.Delay(TimeSpan.FromSeconds(duration));
			Destroy(instance);
		}
		
		public async void MakeDodge(Transform from) {
			forceVector = (transform.position - from.position).normalized * 10f;
			await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
			forceVector *= 0.1f;
			await UniTask.Delay(TimeSpan.FromSeconds(0.8f));
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
			forceVector *= 0.1f;
			await UniTask.Delay(TimeSpan.FromSeconds(0.8f));
			forceVector = Vector2.zero;
		}

		protected override void Die()
		{
			int skill_id = Analyse();		
			roundCount++;
			Debug.Log("был тут");
			Main.instance.resetLevel(skill_id);			
		}

		int Analyse()
		{
			Debug.Log("раундкаунт" + roundCount);
			var attackTypes = new Dictionary<DamageType, int>();
			foreach (var x in attackHistory)
			{
				var type = x.Type;
				switch (type)
				{
					case DamageType.BossMeleeAttack:
					{
						if (attackTypes.ContainsKey(DamageType.BossMeleeAttack))
						{
							attackTypes[DamageType.BossMeleeAttack]++;
						}
						else
						{
							attackTypes.Add(DamageType.BossMeleeAttack, 1);
						}

						break;
					}
					case DamageType.BossAbility1:
					{
						if (attackTypes.ContainsKey(DamageType.BossAbility1))
						{
							attackTypes[DamageType.BossAbility1]++;
						}
						else
						{
							attackTypes.Add(DamageType.BossAbility1, 1);
						}

						break;
					}
					case DamageType.BossAbility2:
					{
						if (attackTypes.ContainsKey(DamageType.BossAbility2))
						{
							attackTypes[DamageType.BossAbility2]++;
						}
						else
						{
							attackTypes.Add(DamageType.BossAbility2, 1);
						}

						break;
					}
					case DamageType.BossAbility3:
					{
						if (attackTypes.ContainsKey(DamageType.BossAbility3))
						{
							attackTypes[DamageType.BossAbility3]++;
						}
						else
						{
							attackTypes.Add(DamageType.BossAbility3, 1);
						}

						break;
					}
					case DamageType.Magma:
					{
						if (attackTypes.ContainsKey(DamageType.Magma))
						{
							attackTypes[DamageType.Magma]++;
						}
						else
						{
							attackTypes.Add(DamageType.Magma, 1);
						}

						break;
					}
				}
				
			}
			int perkValue = -1;

				var keyOfMaxValue = attackTypes.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

				if (roundCount == 0)
				{
					m_ReactiveAbilities.Add(ReactiveAbility.Block);
					perkValue = 0;
				}
				if (roundCount == 1)
				{
					m_ReactiveAbilities.Add(ReactiveAbility.Dodge);
					m_ReactiveAbilities.Add(ReactiveAbility.DodgeCharge);
					perkValue = 2;
				}
				else if (roundCount == 2)
				{
					m_ReactiveAbilities.Add(ReactiveAbility.MagicImmunity);
					perkValue = 3;
				}
				else if (roundCount == 3 && !regenEnabled)
				{
					BaseTask task = new RegenTask();
					task = new RegenTask();
					task.InitTask(this, m_PlayerCharacter, RangeAttackPrefab);
					m_AvailableTasks.Add(task);
					regenEnabled = true;
					perkValue = 6;
				}
				else if (roundCount == 4)
				{
					BaseTask task = new RangeAttackTask();
					task.InitTask(this, m_PlayerCharacter, RangeAttackPrefab);
					m_AvailableTasks.Add(task);
					rangeAttackEnabled = true;
					perkValue = 4;
					if (!m_ReactiveAbilities.Contains(ReactiveAbility.AvoidWater)) {
						m_ReactiveAbilities.Add(ReactiveAbility.AvoidWater);
					}
				}
				else if(roundCount == 5)
				{
					BaseTask task = new ChestOpeningTask();
					task.InitTask(this, m_PlayerCharacter, RangeAttackPrefab);
					m_AvailableTasks.Add(task);
					chestOpeningEnabled = true;
					perkValue = 7;
				}
				else if (roundCount == 6)
				{
					m_ReactiveAbilities.Add(ReactiveAbility.StoneBoots);
					perkValue = 1;
				}
				else if (roundCount == 7)
				{
					m_ReactiveAbilities.Add(ReactiveAbility.ShootBarrels);
					perkValue = 5;
				}
				else if (roundCount == 8)
				{
					perkValue = 8;
                    m_ReactiveAbilities.Add(ReactiveAbility.ImmunityMagma);
				}
				return perkValue;
		}

	}
	
}