using UnityEditor;
using UnityEngine;

namespace TetrisGame.Editor {
	public static class SaveUtility {
		[MenuItem("TetrisGame/Clear")]
		public static void Clear() {
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();
		}
	}
}