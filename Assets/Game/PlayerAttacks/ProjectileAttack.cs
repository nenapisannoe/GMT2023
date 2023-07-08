using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.PlayerAttacks {
	
	public class ProjectileAttack : AttackBase {

		[SerializeField] private Rigidbody2D m_RigidBody;

		private Vector2 direction;

		private bool canFinishAttack;

		public override async UniTask Run() {
			var task = base.Run();
			await task;
			await UniTask.WaitUntil(() => canFinishAttack);
		}

		public override Vector2 CheckPosition(Vector2 mousePos, Vector2 characterPos) {
			var newPos = mousePos - characterPos;
			newPos.Normalize();
			direction = newPos;
			return newPos;
		}

		public override void AttackTrigger() {
			base.AttackTrigger();

			m_RigidBody.velocity = direction * 5f;
		}
		
		protected override void OnTriggerEnter2D(Collider2D other) {
			base.OnTriggerEnter2D(other);
			canFinishAttack = true;
		}

	}
	
}