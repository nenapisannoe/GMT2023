
using UnityEngine;

namespace Game.PlayerAttacks {
	
	public class MeleeAttack : AttackBase {
		
		public override Vector2 CheckPosition(Vector2 mousePos, Vector2 characterPos) {
			var newPos = mousePos - characterPos;
			newPos.Normalize();
			return newPos;
		}
		
	}
	
}