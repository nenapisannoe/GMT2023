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

		private float moveSpeed = 5f;
		private Vector2 moveInput;
		private Vector2 mouseInput;

		private AttackHandle currentAttackhandle;

		private void OnMove(InputValue input) {
			moveInput = input.Get<Vector2>();
		}
		
		private void OnLook(InputValue input) {
			mouseInput = input.Get<Vector2>();
		}

		private void OnFire(InputValue input) {
			var pos = Camera.main.ScreenToWorldPoint(mouseInput);
			SetVelocity(Vector2.zero);
			MakeBasicAttack(BasicAttackPrefab, pos);
		}
		
		private void OnAbility1(InputValue input) {
			var pos = Camera.main.ScreenToWorldPoint(mouseInput);
			SetVelocity(Vector2.zero);
			MakeBasicAttack(Ability1Prefab, pos);
		}
		
		private void OnAbility2(InputValue input) {
			var pos = Camera.main.ScreenToWorldPoint(mouseInput);
			SetVelocity(Vector2.zero);
			MakeBasicAttack(Ability2Prefab, pos);
		}
		
		private void OnAbility3(InputValue input) {
			var pos = Camera.main.ScreenToWorldPoint(mouseInput);
			SetVelocity(Vector2.zero);
			MakeBasicAttack(Ability3Prefab, pos);
		}
		
		
		private void OnAbility4(InputValue input) {
			var pos = Camera.main.ScreenToWorldPoint(mouseInput);
			SetVelocity(Vector2.zero);
			currentAttackhandle = MakeChannelingAttack(Ability4Prefab, pos);
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

		private void FixedUpdate() {
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