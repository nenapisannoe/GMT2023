using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game {
	
	public enum DamageType {
		BossMeleeAttack,
		BossAbility1,
		BossAbility2,
		BossAbility3,
		BossAbility4,
		HeroAbility1,
		HeroAbility2,
		BarrelExplosion,
		Magma
	}

	public class Damage {
		public Character Attacker;
		public DamageType Type;
		public float Value;
	}
	
	public abstract class AttackBase : MonoBehaviour {

		public virtual bool HaveKnockback { get; }

		[SerializeField] protected Animator m_Animator;
		[SerializeField] protected Collider2D m_Collider;
		private static readonly int Play = Animator.StringToHash("Play");
		protected static readonly int End = Animator.StringToHash("End");

		[SerializeField] public DamageType damageType;
		[SerializeField] public float damage = 10;

		private List<HitableObject> attackedTargets = new List<HitableObject>();

		public event Action OnRemoveLock = delegate {};
		public event Action<AttackBase, HitableObject, Damage> OnAttackTarget = delegate {};

		public Damage attackDamage;
		
		private void Awake() {
			m_Collider.enabled = false;
		}

		public void InitAttack(Damage damage) {
			attackDamage = damage;
			ResetAttackedTargets();
		}

		protected void ResetAttackedTargets() {
			attackedTargets.Clear();
			//чтобы себя не жухать
			attackedTargets.Add(attackDamage.Attacker);
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
			var target = other.gameObject.GetComponent<HitableObject>();
			if (target != null && !attackedTargets.Contains(target)) {
				attackedTargets.Add(target);
				OnAttackTarget.Invoke(this, target, attackDamage);
			}
		}
		
	}
	
}