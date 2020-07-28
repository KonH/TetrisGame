using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame {
	public sealed class ActiveFigureManager : MonoBehaviour {
		Figure _current;

		public void ChangeCurrentFigure(Figure figure) {
			Assert.IsNotNull(figure);
			_current = figure;
		}

		public void TryRotate(float angle) {
			if ( !_current ) {
				return;
			}
			_current.Rotate(angle);
		}

		public void TryMove(Vector3 direction) {
			if ( !_current ) {
				return;
			}
			_current.Move(direction);
		}
	}
}