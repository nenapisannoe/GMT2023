
using UnityEngine;

namespace Game.PlayerAttacks {
	
	public class MeleeAttack : AttackBase {
		
		public override Vector2 CheckPosition(Vector2 mousePos, Vector2 characterPos) {
			var newPos = mousePos - characterPos;
			return newPos.normalized * 0.25f;
		}
		
	}
	
}