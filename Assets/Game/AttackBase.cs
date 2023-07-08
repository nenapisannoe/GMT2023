using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game {
	
	public enum DamageType {
		Physical,
		Fire
	}

	public class Damage {

		public DamageType Type;
		public float Value;

	}
	
	public abstract class AttackBase : MonoBehaviour {
		
		[SerializeField] protected Animator m_Animator;
		[SerializeField] private Collider2D m_Collider;
		private static readonly int Play = Animator.StringToHash("Play");
		protected static readonly int End = Animator.StringToHash("End");

		private List<Character> attackedTargets = new List<Character>();

		public event Action OnRemoveLock = delegate {};
		public event Action<Character, Damage> OnAttackTarget = delegate {};

		private Damage attackDamage;
		
		private void Awake() {
			m_Collider.enabled = false;
		}

		public void InitAttack(Damage damage) {
			attackDamage = damage;
		}

		public abstract Vector2 CheckPosition(Vector2 mousePos, Vector2 characterPos);

		public virtual async UniTask Run() {
			var task = AnimationStateHandler.AwaitStateExitEvent(m_Animator);
			m_Animator.SetTrigger(Play);
			
			await task;
		}

		public void RemoveLockTrigger() {
			OnRemoveLock.Invoke();
		}

		public virtual void AttackTrigger() {
			m_Collider.enabled = true;
		}
		
		public virtual void AttackCompleteTrigger() {
			m_Collider.enabled = false;
		}

		protected virtual void OnTriggerEnter2D(Collider2D other) {
			var character = other.gameObject.GetComponent<Character>();
			if (character != null && !attackedTargets.Contains(character)) {
				attackedTargets.Add(character);
				OnAttackTarget.Invoke(character, attackDamage);
			}
		}
		
	}
	
}