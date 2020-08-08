using System.Collections.Generic;
using UnityEngine;

namespace TetrisGame.Logic {
	public sealed class FigureDeconstructor {
		public void Apply(bool[,] field, IReadOnlyList<Vector2Int> positions) {
			foreach ( var position in positions ) {
				field[position.x, position.y] = true;
			}
		}
	}
}