using System;
using UnityEngine;

namespace Game {
	
	public class Main : MonoBehaviour {

		public static Main instance;


		public GameObject skillScreen;

		private void Awake() {
			Debug.Log("[MAIN AWAKE]");
			instance = this;
		}

		private void Start() {
			Debug.Log("[MAIN START]");
		}

		public void showSkillScreen(){
			skillScreen.SetActive(true);
		}

		public void enableAll(){
			InicalizeManager.enableAll();
		}

		public void resetLevel(){
			InicalizeManager.disableAll();
			showSkillScreen();
		}

		public static int GetEpochTime() {
			var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return (int)(DateTime.UtcNow - epochStart).TotalSeconds;
		}

	}
	
}