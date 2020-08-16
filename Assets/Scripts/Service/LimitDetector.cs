using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	public sealed class LimitDetector {
		readonly int _width;

		public LimitDetector(int width) {
			_width = width;
		}

		public bool IsLimitReached(IReadOnlyFigureState figure) {
			for ( var i = 0; i < figure.Elements.Count; i++ ) {
				var element = figure.Elements[i];
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