using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	/// <summary>
	/// Apply fixed 90 degrees rotation
	/// </summary>
	public sealed class FigureRotator {
		public void Rotate(FigureState figure) {
			for ( var i = 0; i < figure.Elements.Count; i++ ) {
				figure.Elements[i] = Rotate(figure.Elements[i]);
			}
		}

		Vector2 Rotate(Vector2 element) {
			return new Vector2(element.y, -element.x);
		}
	}
}