using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
	
	public class PlayerController : Character {
		
		[SerializeField] private Rigidbody2D m_Rigidbody;
		[SerializeField] private Animator m_SpriteAnimator;

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
		
		private void MakeBasicAttack() {
			if (actionsLocked) {
				return;
			}
			actionsLocked = true;
			var handle = AttackManager.Instance.MakeAttack(BasicAttackPrefab, mouseInput, transform.position);
			handle.OnComplete += MakeBasicAttackComplete;
			handle.OnRemoveLock += UnlockActions;
		}

		private void UnlockActions() {
			actionsLocked = false;
		}
		


		private void MakeBasicAttackComplete(AttackHandle handle) {
			handle.OnRemoveLock -= UnlockActions;
			handle.OnComplete -= MakeBasicAttackComplete;
		}

		private void FixedUpdate() {
			var vel = actionsLocked ? Vector2.zero : moveInput * moveSpeed;
			m_SpriteAnimator.SetBool(IsWalking, vel != Vector2.zero);
			m_SpriteAnimator.SetFloat(Vertical, vel.y);
			transform.localScale = vel.x < 0 ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);
			m_Rigidbody.velocity = vel;
		}

	}
	
}