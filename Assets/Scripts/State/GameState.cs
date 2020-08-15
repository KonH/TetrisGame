namespace TetrisGame.State {
	public sealed class GameState : IReadOnlyGameState {
		public FieldState  Field    { get; }
		public FigureState Figure   { get; }
		public SpeedState  Speed    { get; }
		public int         Scores   { get; set; }
		public bool        Finished { get; set; }
		public InputState  Input    { get; set; }

		IReadOnlyFieldState IReadOnlyGameState. Field  => Field;
		IReadOnlyFigureState IReadOnlyGameState.Figure => Figure;
		IReadOnlySpeedState IReadOnlyGameState. Speed  => Speed;

		public GameState(int width, int height, float initialSpeed) {
			Field  = new FieldState(width, height);
			Figure = new FigureState();
			Speed  = new SpeedState(initialSpeed);
		}
	}
}