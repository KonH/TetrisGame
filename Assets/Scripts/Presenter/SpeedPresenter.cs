using TMPro;

namespace TetrisGame.Presenter {
	public sealed class SpeedPresenter {
		readonly TMP_Text _text;

		int _currentLevel;

		public SpeedPresenter(TMP_Text text) {
			_text = text;
		}

		public void Draw(int level) {
			var displayLevel = level + 1;
			if ( _currentLevel == displayLevel ) {
				return;
			}
			_text.text = displayLevel.ToString();
		}
	}
}