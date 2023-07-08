using UnityEngine;

namespace Game.Enemy {
	
	public class ApproachTask : BaseTask {

		protected override int m_Cooldown => 0;
		
		public override bool CanExecuteTask() {
			var dist = GetDistance(executor.transform.position, target.transform.position);
			return dist > 1d;
		}
		
		public override ExecutorTask GetTaskExecutor() {
			return ExecutorTask.MoveToPosition;
		}
		
		public override Vector3 GetMoveToPositionTaskData() {
			//нужно двигаться прямо к цели
			return target.transform.position;
		}
		
	}
	
}