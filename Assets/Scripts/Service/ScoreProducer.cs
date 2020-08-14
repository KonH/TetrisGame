using System.Collections.Generic;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	public sealed class ScoreProducer {
		readonly IReadOnlyList<int> _scorePerLines;

		public ScoreProducer(IReadOnlyList<int> scorePerLines) {
			_scorePerLines = scorePerLines;
		}

		public void AddScores(GameState state, int collapsedLines) {
			var index        = Mathf.Clamp(collapsedLines, 0, _scorePerLines.Count - 1);
			var scorePerLine = _scorePerLines[index];
			state.Scores += scorePerLine;
		}
	}
}