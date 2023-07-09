using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.PlayerAttacks {
	
	public class ChargeAttack : AttackBase {
		
		[SerializeField] private Rigidbody2D m_RigidBody;
		
		public float ChargeDuration = 1f;
		public float ChargeSpeed = 100f;
		public Vector2 CachePos;
		
		public override async UniTask Run() {
			var task = base.Run();
			var chargeTask = ChargeCharacter(attackDamage.Attacker);
			await UniTask.WhenAll(task, chargeTask);
		}

		private async UniTask ChargeCharacter(Character attacker) {
			var vel = CachePos * ChargeSpeed;
			attacker.SetVelocity(vel);
			attacker.SetCharge(true);
			m_RigidBody.velocity = vel;
			await UniTask.Delay(TimeSpan.FromSeconds(ChargeDuration));
			attacker.SetVelocity(Vector2.zero);
			m_RigidBody.velocity = Vector2.zero;
			attacker.SetCharge(false);
			//лок управления снимается тут, а не в анимации
			RemoveLockTrigger();
		}

		public override Vector2 CheckPosition(Vector2 mousePos, Vector2 characterPos) {
			var newPos = mousePos - characterPos;
			CachePos = newPos.normalized;
			return CachePos * 0.25f;
		}
		
	}
	
}