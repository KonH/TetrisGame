using System.Collections.Generic;
using TetrisGame.State;

namespace TetrisGame.Service {
	/// <summary>
	/// Detect full filled lines (contains element at each position in some row)
	/// </summary>
	public sealed class LineDetector {
		public void DetectLines(IReadOnlyFieldState field, List<int> result) {
			result.Clear();
			for ( var y = 0; y < field.Height; y++ ) {
				var isFilled = true;
				for ( var x = 0; x < field.Width; x++ ) {
					if ( !field.GetStateUnsafe(x, y) ) {
						isFilled = false;
						break;
					}
				}
				if ( isFilled ) {
					result.Add(y);
				}
			}
		}
	}
}