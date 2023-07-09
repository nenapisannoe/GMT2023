using System;
using UnityEngine;

namespace Game {
	
	public class Main : MonoBehaviour {

		public static Main instance;
		public PerkSelection selection;

		public GameObject skillScreen;
		public GameObject gameOverScreen;

		private void Awake() {
			Debug.Log("[MAIN AWAKE]");
			instance = this;
		}

		private void Start() {
			Debug.Log("[MAIN START]");
		}

		public void showSkillScreen(int skillId){
			skillScreen.SetActive(true);
			selection.UnlockNewPerk(skillId);
			selection.ShowPerk();
		}

		public void showGameOverScreen(){
			gameOverScreen.SetActive(true);
		}

		public void enableAll(){
			InicalizeManager.enableAll();
		}

		public void resetLevel(int skillId){
			InicalizeManager.disableAll();
			if (skillId != -1){
				showSkillScreen(skillId);
			}else{
				showGameOverScreen();
			}
			
		}

		public static int GetEpochTime() {
			var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return (int)(DateTime.UtcNow - epochStart).TotalSeconds;
		}

	}
	
}