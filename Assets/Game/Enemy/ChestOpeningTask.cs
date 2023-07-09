using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Enemy {
    public class ChestOpeningTask: BaseTask
    {
        protected override int m_Cooldown => 2;

        public override bool CanExecuteTask() {
			if (!base.CanExecuteTask()) {
				return false;
			}
            if ((ChestsStorage.active_chests.Count == 0))
            {
                return false;
            }
            else{
                target = ChestsStorage.active_chests[0];
            }
			
			var dist = GetDistance(executor.transform.position, target.transform.position);
			return dist < 1d;
		}

        public override ExecutorTask GetTaskExecutor() {
			return ExecutorTask.OpenChest;
		}

        public override void CompleteTask() {
            target = null;
		}
    }
}
