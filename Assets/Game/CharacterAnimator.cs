using UnityEngine;

namespace Game {
	
	public class CharacterAnimator : MonoBehaviour {

		public Animator m_Animator;
		private static readonly int Hit = Animator.StringToHash("Hit");
		private static readonly int OpenChest = Animator.StringToHash("OpenChest");

		public void PlayHit() {
			m_Animator.SetTrigger(Hit);
		}

		public void PlayOpenChest()
		{
			m_Animator.SetTrigger(OpenChest);
		}
	}
	
}