using System.Collections.Generic;
using UnityEngine;

namespace TetrisGame.Service {
	public struct GameLoopSettings {
		public readonly int                Width;
		public readonly int                Height;
		public readonly float              InitialSpeed;
		public readonly int                LinesToIncrease;
		public          float              IncreaseValue;
		public readonly Vector2Int[][]     Figures;
		public readonly IReadOnlyList<int> ScorePerLines;
		public readonly int                RandomSeed;

		public GameLoopSettings(
			int width, int height, float initialSpeed, int linesToIncrease, float increaseValue,
			Vector2Int[][] figures, IReadOnlyList<int> scorePerLines, int randomSeed) {
			Width           = width;
			Height          = height;
			InitialSpeed    = initialSpeed;
			LinesToIncrease = linesToIncrease;
			IncreaseValue   = increaseValue;
			Figures         = figures;
			ScorePerLines   = scorePerLines;
			RandomSeed      = randomSeed;
		}
	}
}