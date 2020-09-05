using TetrisGame.Service;
using UnityEngine;

namespace TetrisGame.Settings {
	[CreateAssetMenu]
	public sealed class GameGeneticSettings : ScriptableObject {
		public GeneticSettings Settings;
	}
}