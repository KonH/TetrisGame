using System.Collections.Generic;
using TetrisGame.State;

namespace TetrisGame.Service {
	/// <summary>
	/// Drop all lines above affected lines indexes
	/// </summary>
	public sealed class LineDropper {
		readonly List<int> _sortedLines = new List<int>();

		public void Drop(FieldState field, List<int> affectedLines) {
			_sortedLines.Clear();
			_sortedLines.AddRange(affectedLines);
			// Sort descending to move top lines at first time
			_sortedLines.Sort((x, y) => y.CompareTo(x));
			foreach ( var line in _sortedLines ) {
				for ( var y = line; y < field.Height; y++ ) {
					for ( var x = 0; x < field.Width; x++ ) {
						SwapValues(field, x, y);
					}
				}
			}
			field.IsDirty = true;
		}

		void SwapValues(FieldState field, int x, int y) {
			var hasValue = (y < field.Height - 1);
			var topValue = hasValue && field.Field[x, y + 1];
			field.Field[x, y] = topValue;
			if ( hasValue ) {
				field.Field[x, y + 1] = false;
			}
		}
	}
}