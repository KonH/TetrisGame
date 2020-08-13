namespace TetrisGame.State {
	public sealed class GameState {
		public readonly FieldState  Field;
		public readonly FigureState Figure = new FigureState();

		public GameState(int width, int height) {
			Field = new FieldState(width, height);
		}
	}
}