using System;
using UnityEngine;

namespace Game.Enemy {
	
	public class RangeAttackTask : BaseTask {
		
		protected override int m_Cooldown => 2;

		private ExplosiveBarrelController targetedBarrel = null;
		
		public override bool CanExecuteTask() {
			if (!base.CanExecuteTask()) {
				return false;
			}

			var canShootBarrels = executor.m_ReactiveAbilities.Contains(ReactiveAbility.ShootBarrels);
			if (canShootBarrels && BarrelsStorage.active_barrels.Count > 0) {
				var d = Double.MaxValue;
				targetedBarrel = null;
				foreach (var obj in BarrelsStorage.active_barrels) {
					var gd = GetDistance(executor.transform.position, obj.transform.position);
					if (gd > 3d && d > gd) {
						targetedBarrel = obj;
						d = gd;
					}
				}
				if (targetedBarrel != null) {
					return true;
				}
			}
			
			var dist = GetDistance(executor.transform.position, target.transform.position);
			return dist < 5d;
		}

		public override ExecutorTask GetTaskExecutor() {
			return ExecutorTask.AttackTarget;
		}

		public override Vector3 GetAttackTargetTaskData() {
			if (targetedBarrel != null) {
				var pos = targetedBarrel.transform.position;
				targetedBarrel = null;
				return pos;
			}
			//нужно атаковать прямо в сторону цели
			return target.transform.position;
		}
		
	}
	
}