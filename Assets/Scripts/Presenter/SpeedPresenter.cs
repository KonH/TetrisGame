using TMPro;
using UnityEngine;

namespace TetrisGame.Presenter {
	/// <summary>
	/// Draw speed level and apply animation while it changes
	/// </summary>
	public sealed class SpeedPresenter {
		readonly TMP_Text  _text;
		readonly Animation _animation;

		int _currentLevel;

		public SpeedPresenter(TMP_Text text) {
			_text      = text;
			_animation = _text.GetComponent<Animation>();
		}

		public void Draw(int level) {
			var displayLevel = level + 1;
			if ( _currentLevel == displayLevel ) {
				return;
			}
			_currentLevel = displayLevel;
			_text.text = displayLevel.ToString();
			if ( _animation ) {
				_animation.Play();
			}
		}
	}
}