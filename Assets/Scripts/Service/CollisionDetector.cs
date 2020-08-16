using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	/// <summary>
	/// Detect is any figure element collides with any other field element
	/// </summary>
	public sealed class CollisionDetector {
		public bool HasCollisions(IReadOnlyFieldState field, IReadOnlyFigureState figure) {
			for ( var i = 0; i < figure.Elements.Count; i++ ) {
				var element = figure.Elements[i];
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