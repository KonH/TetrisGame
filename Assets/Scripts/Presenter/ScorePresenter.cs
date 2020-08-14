using TMPro;

namespace TetrisGame.Presenter {
	public sealed class ScorePresenter {
		readonly TMP_Text _text;

		int _currentScores;

		public ScorePresenter(TMP_Text text) {
			_text = text;
		}

		public void Draw(int scores) {
			if ( _currentScores == scores ) {
				return;
			}
			_text.text = scores.ToString("N0");
		}
	}
}