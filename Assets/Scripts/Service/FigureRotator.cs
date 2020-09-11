using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	/// <summary>
	/// Apply fixed 90 degrees rotation
	/// </summary>
	public sealed class FigureRotator {
		public void Rotate(FigureState figure) {
			RotateInternal(figure, true);
		}

		public void RotateBack(FigureState figure) {
			RotateInternal(figure, false);
		}

		void RotateInternal(FigureState figure, bool clockwise) {
			for ( var i = 0; i < figure.Elements.Count; i++ ) {
				figure.Elements[i] = Rotate(figure.Elements[i], clockwise);
			}
		}

		Vector2Int Rotate(Vector2Int element, bool clockwise) {
			return clockwise ? new Vector2Int(element.y, -element.x) : new Vector2Int(-element.y, element.x);
		}
	}
}