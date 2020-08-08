using System.Collections.Generic;
using UnityEngine;

namespace TetrisGame.Logic {
	public sealed class MovementChecker {
		static readonly List<(bool allowed, bool finished)> Results = new List<(bool, bool)>();

		/// <summary>
		/// Calculate movement ability on given field
		/// </summary>
		/// <returns>
		/// allowed - is that move possible (no side collisions),
		/// finished - figure is fallen on other or floor (bottom collision happens) </returns>
		public (bool allowed, bool finished) CalculateAbility(
			bool[,] field, Vector2Int[] figure, Vector2Int direction) {
			var isHorizontalMove = (direction.x != 0);
			var width            = field.GetLength(0);
			var height           = field.GetLength(0);
			Results.Clear();
			var results = Results;
			foreach ( var originPosition in figure ) {
				var newPosition = originPosition + direction;
				if ( (newPosition.x < 0) || (newPosition.x >= width) ) {
					// Side out of bounds
					results.Add((false, false));
					continue;
				}
				if ( newPosition.y <= 0 ) {
					// Vertical out of bounds
					results.Add((true, true));
					continue;
				}
				if ( newPosition.y >= height ) {
					// Initial movement
					results.Add((true, false));
					continue;
				}
				var hasCollision = field[newPosition.x, newPosition.y];
				if ( hasCollision ) {
					// Side or bottom collision
					results.Add(isHorizontalMove ? (false, false) : (false, true));
				}
			}
			var isAllowedAcc  = true;
			var isFinishedAcc = false;
			foreach ( var result in results ) {
				isAllowedAcc  = isAllowedAcc && result.allowed;
				isFinishedAcc = isFinishedAcc || result.finished;
			}
			return (isAllowedAcc, isFinishedAcc);
		}
	}
}