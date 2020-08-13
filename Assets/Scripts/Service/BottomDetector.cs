using TetrisGame.State;

namespace TetrisGame.Service {
	public sealed class BottomDetector {
		public bool IsOnBottom(FigureState figure) {
			foreach ( var element in figure.Elements ) {
				if ( (figure.Origin + element).y <= 0 ) {
					return true;
				}
			}
			return false;
		}
	}
}