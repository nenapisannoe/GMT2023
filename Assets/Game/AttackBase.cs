using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game {
	
	public class AttackBase : MonoBehaviour {
		
		[SerializeField] private Animator m_Animator;
		[SerializeField] private Collider2D m_Collider;
		private static readonly int Play = Animator.StringToHash("Play");

		private List<Character> attackedTargets = new List<Character>();

		public event Action OnRemoveLock = delegate {};
		public event Action<Character> OnAttackTarget = delegate {};

		private void Awake() {
			m_Collider.enabled = false;
		}

		public Vector2 CheckPosition(Vector2 mousePos, Vector2 characterPos)
		{
			var newPos = new Vector3(Mathf.Clamp(mousePos.x, characterPos.x - 1f, characterPos.x + 1f), Mathf.Clamp(mousePos.y, characterPos.y - 1f, characterPos.y + 1f), 0f); 
			// проверяем позицию и корректируем в зависимости от скилла
			return newPos;
		}

		public async UniTask Run() {
			var task = AnimationStateHandler.AwaitStateExitEvent(m_Animator);
			m_Animator.SetTrigger(Play);
			await task;
		}

		public void RemoveLockTrigger() {
			OnRemoveLock.Invoke();
		}

		public void AttackTrigger() {
			m_Collider.enabled = true;
		}
		
		public void AttackCompleteTrigger() {
			m_Collider.enabled = false;
		}
		
		private void OnTriggerEnter2D(Collider2D other) {
			var character = other.gameObject.GetComponent<Character>();
			if (character != null && !attackedTargets.Contains(character)) {
				attackedTargets.Add(character);
				OnAttackTarget.Invoke(character);
			}
		}
		
	}
	
}