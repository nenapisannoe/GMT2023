using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Game {

	public class AttackHandle {
		
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

		public static AttackManager Instance;

		float tiltAngle = 60.0f;
		private void Awake() {
			if (Instance != null) {
				throw new Exception($"Singleton error, this={this}");
			}
			Instance = this;
		}
		
		public AttackHandle MakeAttack(AttackBase attackPrefab, Vector2 targetPosition, Vector2 characterPosition) {
			var attack = Instantiate(attackPrefab);
			targetPosition = attack.CheckPosition(targetPosition, characterPosition);
			attack.transform.position = new Vector2(characterPosition.x + targetPosition.x, characterPosition.y + targetPosition.y);
			var angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
			attack.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
			var handle = new AttackHandle();
			WaitAttackComplete(handle, attack);
			return handle;
		}

		private async void WaitAttackComplete(AttackHandle handle, AttackBase attack) {
			attack.OnRemoveLock += handle.RemoveLock;
			attack.OnAttackTarget += ApplyAttack;
			await attack.Run();
			attack.OnRemoveLock -= handle.RemoveLock;
			attack.OnAttackTarget -= ApplyAttack;
			handle.Complete();
			Destroy(attack.gameObject);
		}

		private void ApplyAttack(Character character) {
			Debug.Log($"Apply attack to {character}");
			character.Hit();
		}
		
	}
	
}