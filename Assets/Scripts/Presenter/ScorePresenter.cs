using TMPro;
using UnityEngine;

namespace TetrisGame.Presenter {
	public sealed class ScorePresenter {
		readonly TMP_Text    _text;
		readonly AudioSource _source;

		int _currentScores;

		public ScorePresenter(TMP_Text text, AudioSource source) {
			_text   = text;
			_source = source;
		}

		public void Draw(int scores) {
			if ( _currentScores == scores ) {
				return;
			}
			_currentScores = scores;
			_text.text = scores.ToString("N0");
			_source.Play();
		}
	}
}