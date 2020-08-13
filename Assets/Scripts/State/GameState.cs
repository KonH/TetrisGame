namespace TetrisGame.State {
	public sealed class GameState {
		public readonly FieldState  Field;
		public readonly FigureState Figure = new FigureState();

		public InputState Input;

		public GameState(int width, int height) {
			Field = new FieldState(width, height);
		}
	}
}