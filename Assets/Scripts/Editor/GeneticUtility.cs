using UnityEditor;
using UnityEngine;

namespace TetrisGame.Editor {
	public static class GeneticUtility {
		[MenuItem("TetrisGame/Genetic/Enable")]
		public static void Enable() {
			PlayerPrefs.SetInt("UseGenetic", 1);
			PlayerPrefs.Save();
		}

		[MenuItem("TetrisGame/Genetic/Disable")]
		public static void Disable() {
			PlayerPrefs.SetInt("UseGenetic", 0);
			PlayerPrefs.Save();
		}
	}
}