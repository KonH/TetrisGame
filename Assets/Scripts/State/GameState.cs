namespace TetrisGame.State {
	public sealed class GameState {
		public readonly FieldState  Field;
		public readonly FigureState Figure;
		public readonly SpeedState  Speed;

		public InputState Input;
		public int        Scores;
		public bool       Finished;
		public int        SpeedLevel;

		public GameState(int width, int height, float initialSpeed) {
			Field  = new FieldState(width, height);
			Figure = new FigureState();
			Speed  = new SpeedState(initialSpeed);
		}
	}
}