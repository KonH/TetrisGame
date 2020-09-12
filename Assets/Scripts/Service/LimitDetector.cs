using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	/// <summary>
	/// Detect that any figure element is out of field range
	/// </summary>
	public sealed class LimitDetector {
		readonly int _width;

		public LimitDetector(int width) {
			_width = width;
		}

		public bool IsLimitReached(IReadOnlyFigureState figure) {
			var x = figure.Origin.x;
			var y = figure.Origin.y;
			for ( var i = 0; i < figure.Elements.Count; i++ ) {
				var element = figure.Elements[i];
				if ( IsLimitReached(x + element.x, y + element.y) ) {
					return true;
				}
			}
			return false;
		}

		bool IsLimitReached(float x, float y) =>
			(y <= 0) || (x < 0) || (x >= _width);
	}
}