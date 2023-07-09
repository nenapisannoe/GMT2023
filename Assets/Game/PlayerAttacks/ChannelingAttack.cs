using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.PlayerAttacks {
	
	public class ChannelingAttack : AttackBase {

		public float HitDelay = 1f;
		
		private bool canFinishAttack;
		private bool hitStarted;
		private static readonly int LockPlay = Animator.StringToHash("LockPlay");

		public override async UniTask Run() {
			RepeatDamageAsync();
			m_Animator.SetBool(LockPlay, true);
			base.Run().Forget();
			await UniTask.WaitUntil(() => canFinishAttack);
		}

		private async void RepeatDamageAsync() {
			while (!canFinishAttack) {
				ResetAttackedTargets();
				m_Collider.enabled = false;
				await UniTask.NextFrame();
				m_Collider.enabled = hitStarted;
				await UniTask.Delay(TimeSpan.FromSeconds(HitDelay));
			}
		}

		public void ChannelComplete() {
			canFinishAttack = true;
			//лок управления снимается тут, а не в анимации
			RemoveLockTrigger();
		}
		
		public override void AttackTrigger() {
			m_Collider.enabled = true;
			hitStarted = true;
		}

		public override Vector2 CheckPosition(Vector2 mousePos, Vector2 characterPos) {
			return Vector2.zero;
		}
		
	}
	
}