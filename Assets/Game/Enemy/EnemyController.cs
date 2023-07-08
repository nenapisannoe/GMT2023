using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy {
	
	public class EnemyController : Character {

		[SerializeField] private Character m_PlayerCharacter;
		
		[Space]
		[Header("Attacks")]
		[SerializeField] private AttackBase BasicAttackPrefab;

		private List<BaseAttackTask> m_AvailableTasks = new List<BaseAttackTask>();
		private BaseAttackTask ActiveTask;
		
		private float moveSpeed = 2f;

		private void Awake() {
			Init();
		}

		public void Init() {
			//var task = new MeleeAttackTask();
			//task.InitTask(this, m_PlayerCharacter);
			//m_AvailableTasks.Add(task);
		}

		private void Update() {
			if (actionsLocked) {
				return;
			}
			EnemyTick();
		}

		private void EnemyTick() {
			if (ActiveTask == null) {
				foreach (var task in m_AvailableTasks) {
					if (task.CanExecuteTask()) {
						ActiveTask = task;
						break;
					}
				}
			}

			if (ActiveTask == null) {
				return;
			}

			RunTask(ActiveTask);
		}

		private void RunTask(BaseAttackTask task) {
			var et = ActiveTask.GetTaskExecutor();
			switch (et) {
				case ExecutorTask.MoveToPosition:
					RunMoveToPositionTask(task.GetMoveToPositionTaskData());
					break;
				
				case ExecutorTask.AttackTarget:
					task.RunTask();
					RunAttackTargetTask(task.GetAttackTargetTaskData());
					task.CompleteTask();
					ActiveTask = null;
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