using UnityEngine;

namespace Game.PlayerAttacks {
	
	public class AreaAttack : AttackBase {
		
		public override Vector2 CheckPosition(Vector2 mousePos, Vector2 characterPos) {
			return Vector2.zero;
		}
		
	}
	
}