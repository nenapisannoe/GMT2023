using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
	
	public class PlayerController : MonoBehaviour {
		
		[SerializeField] private Rigidbody2D m_Rigidbody;
		[SerializeField] private Animator m_Animator;

		[Space]
		[Header("Attacks")]
		[SerializeField] private AttackBase BasicAttackPrefab;

		private float moveSpeed = 5f;
		private Vector2 moveInput;
		private Vector2 mouseInput;

		private bool actionsLocked;
		private static readonly int IsWalking = Animator.StringToHash("IsWalking");
		private static readonly int Vertical = Animator.StringToHash("Vertical");

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
			await AttackManager.Instance.MakeAttack(BasicAttackPrefab, mouseInput);
			actionsLocked = false;
		}

		private void FixedUpdate() {
			var vel = actionsLocked ? Vector2.zero : moveInput * moveSpeed;
			m_Animator.SetBool(IsWalking, vel != Vector2.zero);
			m_Animator.SetFloat(Vertical, vel.y);
			transform.localScale = vel.x < 0 ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);
			m_Rigidbody.velocity = vel;
		}

	}
	
}