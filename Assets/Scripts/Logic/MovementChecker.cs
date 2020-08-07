using UnityEngine;

namespace TetrisGame.Logic {
	public sealed class MovementChecker {
		/// <summary>
		/// Calculate movement ability on given field
		/// </summary>
		/// <returns>
		/// allowed - is that move possible (no side collisions),
		/// finished - figure is fallen on other or floor (bottom collision happens) </returns>
		public (bool allowed, bool finished) CalculateAbility(
			bool[,] field, Vector2Int[] figure, Vector2Int direction) {
			var isVerticalMove = (direction.y != 0);
			var width = field.GetLength(0);
			var height = field.GetLength(0);
			foreach ( var originPosition in figure ) {
				var newPosition = originPosition + direction;
				if ( (newPosition.x < 0) || (newPosition.x >= width) ) {
					// Side out of bounds
					return (false, false);
				}
				if ( (newPosition.y <= 0) || (newPosition.y >= height) ) {
					// Vertical out of bounds
					return (true, true);
				}
				var hasCollision = field[newPosition.x, newPosition.y];
				if ( hasCollision ) {
					// Bottom or side collision
					return isVerticalMove ? (false, true) : (false, false);
				}
			}
			// Happy path
			return (true, false);
		}
	}
}