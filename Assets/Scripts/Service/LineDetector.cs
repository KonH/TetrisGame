using System.Collections.Generic;
using TetrisGame.State;

namespace TetrisGame.Service {
	public sealed class LineDetector {
		public void DetectLines(IReadOnlyFieldState field, List<int> result) {
			result.Clear();
			for ( var y = 0; y < field.Height; y++ ) {
				var isFilled = true;
				for ( var x = 0; x < field.Width; x++ ) {
					if ( !field.GetState(x, y) ) {
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