using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
	
	public class PlayerController : Character {

		[Space]
		[Header("Attacks")]
		[SerializeField] private AttackBase BasicAttackPrefab;

		private float moveSpeed = 5f;
		private Vector2 moveInput;
		private Vector2 mouseInput;

		private void OnMove(InputValue input) {
			moveInput = input.Get<Vector2>();
		}
		
		private void OnLook(InputValue input) {
			mouseInput = input.Get<Vector2>();
		}

		private void OnFire(InputValue input) {
			var pos = Camera.main.ScreenToWorldPoint(mouseInput);
			MakeBasicAttack(BasicAttackPrefab, pos);
		}

		private void FixedUpdate() {
			var vel = actionsLocked ? Vector2.zero : moveInput * moveSpeed;
			SetVelocity(vel);
		}

	}
	
}