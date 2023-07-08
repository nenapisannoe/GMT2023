using System;
using UnityEngine;

namespace Game {
	
	public class Main : MonoBehaviour {

		private void Awake() {
			Debug.Log("[MAIN AWAKE]");
		}

		private void Start() {
			Debug.Log("[MAIN START]");
		}

		public static int GetEpochTime() {
			var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return (int)(DateTime.UtcNow - epochStart).TotalSeconds;
		}

	}
	
}