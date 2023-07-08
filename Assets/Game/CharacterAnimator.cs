using UnityEngine;

namespace Game {
	
	public class CharacterAnimator : MonoBehaviour {

		[SerializeField] private Animator m_Animator;
		private static readonly int Hit = Animator.StringToHash("Hit");

		public void PlayHit() {
			m_Animator.SetTrigger(Hit);
		}

	}
	
}