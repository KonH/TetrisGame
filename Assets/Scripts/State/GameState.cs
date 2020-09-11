namespace TetrisGame.State {
	public sealed class GameState : IReadOnlyGameState {
		public FieldState  Field        { get; }
		public FigureState Figure       { get; }
		public SpeedState  Speed        { get; }
		public RecordState Records      { get; }
		public int         Scores       { get; set; }
		public int         ClearedLines { get; set; }
		public bool        Finished     { get; set; }
		public InputState  Input        { get; set; }
		public int         FitCount     { get; set; }

		IReadOnlyFieldState  IReadOnlyGameState.Field   => Field;
		IReadOnlyFigureState IReadOnlyGameState.Figure  => Figure;
		IReadOnlySpeedState  IReadOnlyGameState.Speed   => Speed;
		IReadOnlyRecordState IReadOnlyGameState.Records => Records;

		public GameState(int width, int height, float initialSpeed) {
			Field   = new FieldState(width, height);
			Figure  = new FigureState();
			Speed   = new SpeedState(initialSpeed);
			Records = new RecordState();
		}

		public void Clone(IReadOnlyGameState other) {
			Field.Clone(other.Field);
			Figure.Clone(other.Figure);
			Speed.Clone(other.Speed);
			Records.Clone(other.Records);
			Scores       = other.Scores;
			ClearedLines = other.ClearedLines;
			Finished     = other.Finished;
			Input        = other.Input;
			FitCount     = other.FitCount;
		}
	}
}