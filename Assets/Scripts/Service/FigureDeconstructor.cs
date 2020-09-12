using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	/// <summary>
	/// Places figure with floating point elements into discrete field
	/// </summary>
	public sealed class FigureDeconstructor {
		public void Place(FieldState field, IReadOnlyFigureState figure) {
			for ( var i = 0; i < figure.Elements.Count; i++ ) {
				var element = figure.Elements[i];
				Place(field, figure.Origin + element);
			}
		}

		void Place(FieldState field, Vector2 position) {
			var x = Mathf.FloorToInt(position.x);
			var y = Mathf.FloorToInt(position.y);
			if ( (x < 0) || (x >= field.Width) ) {
				return;
			}
			if ( (y < 0) || (y >= field.Height) ) {
				return;
			}
			field.Field[x, y] = true;
			field.IsDirty = true;
		}
	}
}