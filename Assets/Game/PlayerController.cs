using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
	
	public class PlayerController : MonoBehaviour {

		[SerializeField] private Rigidbody2D m_Rigidbody;

		[Space]
		[Header("Attacks")]
		[SerializeField] private AttackBase BasicAttackPrefab;

		private float moveSpeed = 5f;
		private Vector2 moveInput;
		private Vector2 mouseInput;

		private bool actionsLocked;

		private void OnMove(InputValue input) {
			moveInput = input.Get<Vector2>();
		}
		
		private void OnLook(InputValue input) {
			mouseInput = input.Get<Vector2>();
		}

		private void OnFire(InputValue input) {
			MakeBasicAttack();
		}
		
		private async void MakeBasicAttack() {
			if (actionsLocked) {
				return;
			}
			actionsLocked = true;
			await AttackManager.Instance.MakeAttack(BasicAttackPrefab, mouseInput, transform.position);
			actionsLocked = false;
		}

		private void FixedUpdate() {
			var vel = actionsLocked ? Vector2.zero : moveInput * moveSpeed;
			m_Rigidbody.velocity = vel;
		}

	}
	
}