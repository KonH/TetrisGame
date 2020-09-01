using UnityEditor;
using UnityEngine;

namespace TetrisGame.Editor {
	public static class SnapshotUtility {
		[MenuItem("TetrisGame/Snapshot/Enable")]
		public static void Enable() {
			PlayerPrefs.SetInt("SaveSnapshot", 1);
			PlayerPrefs.Save();
		}

		[MenuItem("TetrisGame/Snapshot/Disable")]
		public static void Disable() {
			PlayerPrefs.SetInt("SaveSnapshot", 0);
			PlayerPrefs.Save();
		}
	}
}