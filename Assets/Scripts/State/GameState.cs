namespace TetrisGame.State {
	public sealed class GameState {
		public readonly FieldState  Field;
		public readonly FigureState Figure = new FigureState();

		public InputState Input;
		public int        Scores;
		public bool       Finished;

		public GameState(int width, int height) {
			Field = new FieldState(width, height);
		}
	}
}