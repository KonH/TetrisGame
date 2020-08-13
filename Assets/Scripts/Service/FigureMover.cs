using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	public sealed class FigureMover {
		public void Move(FigureState figureState, Vector2 offset) {
			figureState.Origin += offset;
		}
	}
}