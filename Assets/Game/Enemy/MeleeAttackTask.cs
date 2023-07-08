using UnityEngine;

namespace Game.Enemy {

	public enum ExecutorTask {
		MoveToPosition,
		AttackTarget
	}

	public class MeleeAttackTask : BaseAttackTask {

		protected override int m_Cooldown => 2;

		public override ExecutorTask GetTaskExecutor() {
			var dist = GetDistance(executor.transform.position, target.transform.position);
			if (dist > 1d) {
				return ExecutorTask.MoveToPosition;
			}
			
			return ExecutorTask.AttackTarget;
		}
		
		public override Vector3 GetMoveToPositionTaskData() {
			//нужно двигаться прямо к цели
			return target.transform.position;
		}

		public override Vector3 GetAttackTargetTaskData() {
			//нужно атаковать прямо в сторону цели
			return target.transform.position;
		}

	}
	
}