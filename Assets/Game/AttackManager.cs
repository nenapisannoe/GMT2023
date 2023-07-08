using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game {
	
	public class AttackManager : MonoBehaviour {

		public static AttackManager Instance;

		private void Awake() {
			if (Instance != null) {
				throw new Exception($"Singleton error, this={this}");
			}
			Instance = this;
		}

		public async UniTask MakeAttack(AttackBase attackPrefab, Vector2 position) {
			var attack = Instantiate(attackPrefab);
			var pos = Camera.main.ScreenToWorldPoint(position);
			pos.z = 0f;
			pos = attack.CheckPosition(pos);
			attack.transform.position = pos;
			await attack.Run();
			Destroy(attack.gameObject);
		}
		
	}
	
}