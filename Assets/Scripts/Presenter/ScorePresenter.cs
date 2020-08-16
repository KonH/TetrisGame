using TMPro;
using UnityEngine;

namespace TetrisGame.Presenter {
	/// <summary>
	/// Draw scores and apply animation while it changes
	/// </summary>
	public sealed class ScorePresenter {
		readonly TMP_Text    _text;
		readonly Animation   _animation;
		readonly AudioSource _source;

		int _currentScores;

		public ScorePresenter(TMP_Text text, AudioSource source) {
			_text      = text;
			_animation = _text.GetComponent<Animation>();
			_source    = source;
		}

		public void Draw(int scores) {
			if ( _currentScores == scores ) {
				return;
			}
			_currentScores = scores;
			_text.text = scores.ToString("N0");
			_source.Play();
			if ( _animation ) {
				_animation.Play();
			}
		}
	}
}