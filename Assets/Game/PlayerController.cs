using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
	
	public class PlayerController : MonoBehaviour {

		[SerializeField] private Rigidbody2D m_Rigidbody;

		private float moveSpeed = 5f;
		private Vector2 moveInput;

		private void OnMove(InputValue input) {
			moveInput = input.Get<Vector2>();
		}

		private void FixedUpdate() {
			m_Rigidbody.velocity = moveInput * moveSpeed;
			
		}

	}
	
}