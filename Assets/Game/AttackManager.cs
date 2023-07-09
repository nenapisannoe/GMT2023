using System;
using Game.Enemy;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Game {

	public class AttackHandle {

		public AttackBase Attack;
		public event Action OnRemoveLock = delegate {};
		public event Action<AttackHandle> OnComplete = delegate {};

		public void RemoveLock() {
			OnRemoveLock.Invoke();
		}
		
		public void Complete() {
			OnComplete.Invoke(this);
		}

	}
	
	public class AttackManager : MonoBehaviour {

		[SerializeField] private EnemyController EnemyController;

		public static AttackManager Instance;

		float tiltAngle = 60.0f;
		private void Awake() {
			if (Instance != null) {
				throw new Exception($"Singleton error, this={this}");
			}
			Instance = this;
		}
		
		public AttackHandle MakeAttack(Character attacker, AttackBase attackPrefab, Vector2 targetPosition) {
			var attack = Instantiate(attackPrefab);
			attack.InitAttack(new Damage {
				Attacker = attacker,
				Type = attackPrefab.damageType,
				Value = attackPrefab.damage
			});
			var characterPosition = attacker.transform.position;
			targetPosition = attack.CheckPosition(targetPosition, characterPosition);
			attack.transform.position = new Vector2(characterPosition.x + targetPosition.x, characterPosition.y + targetPosition.y);
			var angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
			attack.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
			var handle = new AttackHandle();
			handle.Attack = attack;
			WaitAttackComplete(handle);
			if (attacker is PlayerController player) {
				EnemyController.Notify(player, attack);
			}
			return handle;
		}

		private async void WaitAttackComplete(AttackHandle handle) {
			var attack = handle.Attack;
			attack.OnRemoveLock += handle.RemoveLock;
			attack.OnAttackTarget += ApplyAttack;
			await attack.Run();
			attack.OnRemoveLock -= handle.RemoveLock;
			attack.OnAttackTarget -= ApplyAttack;
			handle.Complete();
			handle.Attack = null;
			Destroy(attack.gameObject);
		}

		private void ApplyAttack(AttackBase attack, HitableObject target, Damage attackDamage) {
			Debug.Log($"Apply attack to {target}");
			target.attakMe(attackDamage);
			if (attack.HaveKnockback) {
				target.Knockback(attackDamage.Attacker.transform);
			}
		}
		
	}
	
}