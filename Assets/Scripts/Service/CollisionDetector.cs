using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	public sealed class CollisionDetector {
		public bool HasCollisions(IReadOnlyFieldState field, IReadOnlyFigureState figure) {
			foreach ( var element in figure.Elements ) {
				if ( HasCollisions(field, figure.Origin + element) ) {
					return true;
				}
			}
			return false;
		}

		bool HasCollisions(IReadOnlyFieldState field, Vector2 position) {
			var x = Mathf.FloorToInt(position.x);
			var y = Mathf.FloorToInt(position.y);
			if ( (x < 0) || (x >= field.Width) ) {
				return false;
			}
			if ( (y < 0) || (y >= field.Height) ) {
				return false;
			}
			return field.GetState(x, y);
		}
	}
}