using UnityEngine;

namespace Game.Enemy {

	public class MeleeAttackTask : BaseTask {

		protected override int m_Cooldown => 2;
		
		public override bool CanExecuteTask() {
			if (!base.CanExecuteTask()) {
				return false;
			}
			
			var dist = GetDistance(executor.transform.position, target.transform.position);
			return dist < 1d;
		}

		public override ExecutorTask GetTaskExecutor() {
			return ExecutorTask.AttackTarget;
		}

		public override Vector3 GetAttackTargetTaskData() {
			//нужно атаковать прямо в сторону цели
			return target.transform.position;
		}

	}
	
}