namespace Game.Enemy {
	
	public class RegenTask : BaseTask {

		public int RegenPower = 5;
		public int RegenDuration = 5;
		protected override int m_Cooldown => 15;
		
		public override bool CanExecuteTask() {
			if (!base.CanExecuteTask()) {
				return false;
			}
			
			return executor.currentHealth < executor.maxHealth * 0.5f;
		}
		
		public override ExecutorTask GetTaskExecutor() {
			return ExecutorTask.Regen;
		}
		
	}
	
}