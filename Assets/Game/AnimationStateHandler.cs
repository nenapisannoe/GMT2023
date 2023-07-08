using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game {
	
	[SharedBetweenAnimators]
	public class AnimationStateHandler : StateMachineBehaviour {
		
		private class AnimationStateEventHandler {

			public int InstanceID;
			private UniTaskCompletionSource<AnimatorStateInfo> TaskSource = new UniTaskCompletionSource<AnimatorStateInfo>();
			public UniTask<AnimatorStateInfo> Task => TaskSource.Task;

			public void OnStateEvent(int instanceID, AnimatorStateInfo state) {
				if (instanceID == InstanceID) {
					TaskSource.TrySetResult(state);
				}
			}

		}
		
		public static event Action<int, AnimatorStateInfo> StateEnterEvent = delegate {};
		public static event Action<int, AnimatorStateInfo> StateExitEvent = delegate {};
		
		public static async UniTask<AnimatorStateInfo> AwaitStateEnterEvent(Animator animator) {
			var handler = new AnimationStateEventHandler();
			handler.InstanceID = animator.GetInstanceID();
			StateEnterEvent += handler.OnStateEvent;
			var result = await handler.Task;
			StateEnterEvent -= handler.OnStateEvent;
			return result;
		}

		public static async UniTask<AnimatorStateInfo> AwaitStateExitEvent(Animator animator) {
			var handler = new AnimationStateEventHandler();
			handler.InstanceID = animator.GetInstanceID();
			StateExitEvent += handler.OnStateEvent;
			var result = await handler.Task;
			StateExitEvent -= handler.OnStateEvent;
			return result;
		}

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			StateEnterEvent.Invoke(animator.GetInstanceID(), stateInfo);
		}
		
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			StateExitEvent.Invoke(animator.GetInstanceID(), stateInfo);
		}

	}
}