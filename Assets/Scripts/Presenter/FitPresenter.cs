using UnityEngine;

namespace TetrisGame.Presenter {
	public sealed class FitPresenter {
		readonly AudioSource _source;

		int _currentFits;

		public FitPresenter(AudioSource source) {
			_source = source;
		}

		public void Draw(int fits) {
			if ( fits == _currentFits ) {
				return;
			}
			_currentFits = fits;
			_source.Play();
		}
	}
}