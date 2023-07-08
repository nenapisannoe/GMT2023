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

		public async UniTask MakeAttack(AttackBase attackPrefab, Vector2 mousePosition, Vector2 characterPosition) {
			var attack = Instantiate(attackPrefab);
			var pos = Camera.main.ScreenToWorldPoint(mousePosition);
			pos.z = 0f;
			pos = attack.CheckPosition(pos, characterPosition);
			attack.transform.position = pos;
			await attack.Run();
			Destroy(attack.gameObject);
		}
		
	}
	
}