using System;
using UnityEngine;

namespace Game.Enemy {
	
	public enum ExecutorTask {
		MoveToPosition,
		AttackTarget
	}
	
	public abstract class BaseTask {
		
		private int lastUseTime;
		protected abstract int m_Cooldown { get; }
		public bool IsCooldown => Main.GetEpochTime() - lastUseTime < m_Cooldown;

		protected Character executor;
		protected Character target;
		public AttackBase AttackPrefab;

		public void InitTask(Character executor, Character target, AttackBase attackPrefab) {
			this.executor = executor;
			this.target = target;
			AttackPrefab = attackPrefab;
		}

		public virtual bool CanExecuteTask() {
			return !IsCooldown;
		}

		public abstract ExecutorTask GetTaskExecutor();

		public virtual Vector3 GetMoveToPositionTaskData() {
			return Vector3.zero;
		}

		public virtual Vector3 GetAttackTargetTaskData() {
			return Vector3.zero;
		}

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