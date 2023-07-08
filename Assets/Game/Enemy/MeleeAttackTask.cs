using System;
using UnityEngine;

namespace Game.Enemy {

	public class MeleeAttackTask : ITask {

		private Character executor;
		private Character target;

		private bool m_IsComplete;

		public void InitTask(Character executor, Character target) {
			this.executor = executor;
			this.target = target;
		}

		public void Clear() {
			executor = target = null;
		}

		public ExecutorTask GetTaskExecutor() {
			var dist = GetDistance(executor.transform.position, target.transform.position);
			if (dist > 1d) {
				return ExecutorTask.MoveToPosition;
			}
			
			return ExecutorTask.AttackTarget;
		}

		public Vector3 GetMoveToPositionTaskData() {
			//нужно двигаться прямо к цели
			return target.transform.position;
		}

		public Vector3 GetAttackTargetTaskData() {
			//нужно атаковать прямо в сторону цели
			return target.transform.position;
		}

		public void RunTask() {
			if (m_IsComplete) {
				throw new Exception("already complete");
			}
			
			//сначала нужно подойти к цели
			
		}

		private void MoveToTargetSubtask() {
			
		}

		private double GetDistance(Vector3 p1, Vector3 p2) {
			return Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));
		}

	}
	
}