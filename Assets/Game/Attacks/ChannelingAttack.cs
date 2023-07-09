using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.PlayerAttacks {
	
	public class ChannelingAttack : AttackBase {

		public float HitDelay = 1f;
		public int MaxHits = 5;
		
		private bool canFinishAttack;
		private int hits;
		private static readonly int LockPlay = Animator.StringToHash("LockPlay");

		public override async UniTask Run() {
			RepeatDamageAsync();
			m_Animator.SetBool(LockPlay, true);
			base.Run().Forget();
			await UniTask.WaitUntil(() => canFinishAttack);
		}

		private async void RepeatDamageAsync() {
			while (!canFinishAttack) {
				await UniTask.Delay(TimeSpan.FromSeconds(HitDelay));
				ResetAttackedTargets();
				m_Collider.enabled = false;
				await UniTask.NextFrame();
				m_Collider.enabled = true;
				hits++;
				if (hits == MaxHits) {
					InterruptChannel();
				}
			}
		}

		public void InterruptChannel() {
			canFinishAttack = true;
			//лок управления снимается тут, а не в анимации
			RemoveLockTrigger();
		}

		public override Vector2 CheckPosition(Vector2 mousePos, Vector2 characterPos) {
			return Vector2.zero;
		}
		
	}
	
}