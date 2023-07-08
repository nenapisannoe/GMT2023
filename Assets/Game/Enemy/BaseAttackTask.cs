using System;
using UnityEngine;

namespace Game.Enemy {
	
	public abstract class BaseAttackTask {
		
		private int lastUseTime;
		protected abstract int m_Cooldown { get; }
		public bool IsCooldown => Main.GetEpochTime() - lastUseTime < m_Cooldown;

		protected Character executor;
		protected Character target;

		public void InitTask(Character executor, Character target) {
			this.executor = executor;
			this.target = target;
		}

		public bool CanExecuteTask() {
			return !IsCooldown;
		}

		public abstract ExecutorTask GetTaskExecutor();

		public abstract Vector3 GetMoveToPositionTaskData();

		public abstract Vector3 GetAttackTargetTaskData();

		public virtual void RunTask() {
			lastUseTime = Main.GetEpochTime();
		}
		
		public virtual void CompleteTask() {
			
		}

		protected double GetDistance(Vector3 p1, Vector3 p2) {
			return Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));
		}
		
	}
	
}