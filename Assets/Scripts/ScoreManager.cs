using UnityEngine;
using UnityEngine.Events;

namespace TetrisGame {
	public sealed class ScoreManager : MonoBehaviour {
		[SerializeField]
		int[] _scorePerLines;

		[SerializeField]
		UnityEvent<int> _scoreIncreased;

		int _score;

		public int Score => _score;

		public void TryIncreaseScore(int collapsedLines) {
			var index = Mathf.Clamp(collapsedLines, 0, _scorePerLines.Length - 1);
			var scores = _scorePerLines[index];
			if ( scores <= 0 ) {
				return;
			}
			_score += scores;
			_scoreIncreased.Invoke(scores);
		}
	}
}