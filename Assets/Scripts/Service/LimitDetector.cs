using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	public sealed class LimitDetector {
		readonly int _width;

		public LimitDetector(int width) {
			_width = width;
		}

		public bool IsLimitReached(FigureState figure) {
			foreach ( var element in figure.Elements ) {
				if ( IsLimitReached(figure.Origin + element) ) {
					return true;
				}
			}
			return false;
		}

		bool IsLimitReached(Vector2 position) =>
			(position.y <= 0) || (position.x < 0) || (position.x >= _width);
	}
}