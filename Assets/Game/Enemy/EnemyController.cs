using System;
using UnityEngine;

namespace Game.Enemy {
	
	public class EnemyController : Character {

		[SerializeField] private Character m_PlayerCharacter;
		
		[Space]
		[Header("Attacks")]
		[SerializeField] private AttackBase BasicAttackPrefab;

		private MeleeAttackTask MeleeAttackTask = new MeleeAttackTask();
		
		private float moveSpeed = 2f;

		private void Update() {
			if (actionsLocked) {
				return;
			}
			MeleeAttackTask.InitTask(this, m_PlayerCharacter);
			EnemyTick();
		}

		private void EnemyTick() {
			//тут выбираем какую таску сейчас выполнять, пока что одна
			var et = MeleeAttackTask.GetTaskExecutor();
			RunExecutorTask(MeleeAttackTask, et);
		}

		private void RunExecutorTask(MeleeAttackTask task, ExecutorTask et) {
			switch (et) {
				case ExecutorTask.MoveToPosition:
					RunMoveToPositionTask(task.GetMoveToPositionTaskData());
					break;
				
				case ExecutorTask.AttackTarget:
					RunAttackTargetTask(task.GetAttackTargetTaskData());
					break;
				
				default:
					throw new ArgumentOutOfRangeException(nameof(et), et, null);
				
			}
		}

		private void RunMoveToPositionTask(Vector3 target) {
			var dir = target - transform.position;
			var vel = (Vector2)(dir.normalized * moveSpeed);
			SetVelocity(vel);
		}

		private void RunAttackTargetTask(Vector3 target) {
			SetVelocity(Vector2.zero);
			MakeBasicAttack(BasicAttackPrefab, target);
		}

	}
	
}