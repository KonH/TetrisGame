namespace TetrisGame.Logic {
	public struct SessionState {
		public ControlsState Controls;
		public FigureState   Figure;
		public bool[,]       Field;
	}
}