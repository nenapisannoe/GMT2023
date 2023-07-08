using System;
using UnityEngine;

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

		private void Awake() {
			if (Instance != null) {
				throw new Exception($"Singleton error, this={this}");
			}
			Instance = this;
		}

		public AttackHandle MakeAttack(AttackBase attackPrefab, Vector2 mousePosition, Vector2 characterPosition) {
			var attack = Instantiate(attackPrefab);
			var pos = Camera.main.ScreenToWorldPoint(mousePosition);
			pos.z = 0f;
			pos = attack.CheckPosition(pos, characterPosition);
			attack.transform.position = pos;
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
		}
		
	}
	
}