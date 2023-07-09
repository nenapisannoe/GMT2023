using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.PlayerAttacks {
	
	public class ProjectileAttack : AttackBase {

		[SerializeField] private Rigidbody2D m_RigidBody;
		public float ProjectileSpeed = 5f;

		private Vector2 direction;

		private bool canFinishAttack;

		public override async UniTask Run() {
			var task = base.Run();
			await task;
			await UniTask.WaitUntil(() => canFinishAttack);
			var endTask = ProjectileEnd();
			await endTask;
		}
		
		public virtual async UniTask ProjectileEnd() {
			var task = AnimationStateHandler.AwaitStateExitEvent(m_Animator);
			m_Animator.SetTrigger(End);
			
			await task;
		}

		public override Vector2 CheckPosition(Vector2 mousePos, Vector2 characterPos) {
			var newPos = mousePos - characterPos;
			newPos.Normalize();
			direction = newPos;
			return newPos;
		}

		public override void AttackTrigger() {
			base.AttackTrigger();

			m_RigidBody.velocity = direction * ProjectileSpeed;
		}
		
		protected override void OnTriggerEnter2D(Collider2D other) {
			base.OnTriggerEnter2D(other);
			var target = other.gameObject.GetComponent<HitableObject>();
			if (target != null){
				canFinishAttack = true;
				Destroy(gameObject, 0.5f);
			}
			
		}

	}
	
}