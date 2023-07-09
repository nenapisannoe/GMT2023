using Game.PlayerAttacks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
	
	public class PlayerController : Character {

		[Space]
		[Header("Attacks")]
		[SerializeField] private AttackBase BasicAttackPrefab;
		[SerializeField] private AttackBase Ability1Prefab;
		[SerializeField] private AttackBase Ability2Prefab;
		[SerializeField] private AttackBase Ability3Prefab;
		[SerializeField] private AttackBase Ability4Prefab;

		[Space]
		[Header("Attacks cooldowns")]
		[SerializeField] private float BasicAttackCooldown;
		[SerializeField] private float Ability1Cooldown;
		[SerializeField] private float Ability2Cooldown;
		[SerializeField] private float Ability3Cooldown;
		[SerializeField] private float Ability4Cooldown;

		private float moveSpeed = 3f;
		private Vector2 moveInput;
		private Vector2 mouseInput;

		private AttackHandle currentAttackhandle;

		private int BasicAttackLast_use = 0;
		private int Ability1CoolLast_use = 0;
		private int Ability2CoolLast_use = 0;
		private int Ability3CoolLast_use = 0;
		private int Ability4CoolLast_use = 0;

		private void OnMove(InputValue input) {
			moveInput = input.Get<Vector2>();
		}
		
		private void OnLook(InputValue input) {
			mouseInput = input.Get<Vector2>();
		}

		private void OnFire(InputValue input) {
			if ((BasicAttackLast_use == 0) || (Main.GetEpochTime() - BasicAttackLast_use > BasicAttackCooldown)){
				var pos = Camera.main.ScreenToWorldPoint(mouseInput);
				SetVelocity(Vector2.zero);
				MakeBasicAttack(BasicAttackPrefab, pos);

				BasicAttackLast_use = Main.GetEpochTime();
			}
			if (BasicAttackLast_use == 0){
				BasicAttackLast_use = Main.GetEpochTime();
			}
		}

		public void OnEnable(){
			base.OnEnable();
        	BasicAttackLast_use = 0;
			Ability1CoolLast_use = 0;
			Ability2CoolLast_use = 0;
			Ability3CoolLast_use = 0;
			Ability4CoolLast_use = 0;


    	}
		
		private void OnAbility1(InputValue input) {
			if ((Ability1CoolLast_use == 0) || (Main.GetEpochTime() - Ability1CoolLast_use > Ability1Cooldown)){
				var pos = Camera.main.ScreenToWorldPoint(mouseInput);
				SetVelocity(Vector2.zero);
				MakeBasicAttack(Ability1Prefab, pos);

				Ability1CoolLast_use = Main.GetEpochTime();
			}
			if (Ability1CoolLast_use == 0){
				Ability1CoolLast_use = Main.GetEpochTime();
			}
			
		}
		
		private void OnAbility2(InputValue input) {
			if ((Ability2CoolLast_use == 0) || (Main.GetEpochTime() - Ability2CoolLast_use > Ability2Cooldown)){
				var pos = Camera.main.ScreenToWorldPoint(mouseInput);
				SetVelocity(Vector2.zero);
				MakeBasicAttack(Ability2Prefab, pos);

				Ability2CoolLast_use = Main.GetEpochTime();
			}
			if (Ability2CoolLast_use == 0){
				Ability2CoolLast_use = Main.GetEpochTime();
			}
		}
		
		private void OnAbility3(InputValue input) {
			if ((Ability3CoolLast_use == 0) || (Main.GetEpochTime() - Ability3CoolLast_use > Ability3Cooldown)){
				var pos = Camera.main.ScreenToWorldPoint(mouseInput);
				SetVelocity(Vector2.zero);
				MakeBasicAttack(Ability3Prefab, pos);

				Ability3CoolLast_use = Main.GetEpochTime();
			}
			if (Ability3CoolLast_use == 0){
				Ability3CoolLast_use = Main.GetEpochTime();
			}
		}
		
		
		private void OnAbility4(InputValue input) {
			if ((Ability4CoolLast_use == 0) || (Main.GetEpochTime() - Ability4CoolLast_use > Ability4Cooldown)){
				var pos = Camera.main.ScreenToWorldPoint(mouseInput);
				SetVelocity(Vector2.zero);
				currentAttackhandle = MakeChannelingAttack(Ability4Prefab, pos);

				Ability4CoolLast_use = Main.GetEpochTime();
			}
			if (Ability4CoolLast_use == 0){
				Ability4CoolLast_use = Main.GetEpochTime();
			}
		}

		private AttackHandle MakeChannelingAttack(AttackBase attackPrefab, Vector2 target) {
			if (actionsLocked) {
				return null;
			}
			actionsLocked = true;
			var handle = AttackManager.Instance.MakeAttack(this, attackPrefab, target);
			handle.OnComplete += MakeChannelingAttackComplete;
			handle.OnRemoveLock += UnlockActions;
			return handle;
		}
		
		private void MakeChannelingAttackComplete(AttackHandle handle) {
			handle.OnRemoveLock -= UnlockActions;
			handle.OnComplete -= MakeChannelingAttackComplete;
		}

		private void Update() {
			if (actionsLocked) {
				return;
			}
			var vel = actionsLocked ? Vector2.zero : moveInput * moveSpeed;
			SetVelocity(vel);
		}

		public override void Hit(Damage damage) {
			base.Hit(damage);
			if (currentAttackhandle is { Attack: ChannelingAttack channelingAttack }) {
				channelingAttack.InterruptChannel();
			}
		}

		

	}
	
}