using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	/// <summary>
	/// Detect is any figure element collides with any other field element
	/// </summary>
	public sealed class CollisionDetector {
		public bool HasCollisions(IReadOnlyFieldState field, IReadOnlyFigureState figure) {
			var originX = Mathf.FloorToInt(figure.Origin.x);
			var originY = Mathf.FloorToInt(figure.Origin.y);
			for ( var i = 0; i < figure.Elements.Count; i++ ) {
				var element = figure.Elements[i];
				if ( field.GetState(originX + element.x, originY + element.y) ) {
					return true;
				}
			}
			return false;
		}
	}
}