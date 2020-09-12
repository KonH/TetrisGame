namespace TetrisGame.State {
	public interface IReadOnlyGameState {
		IReadOnlyFieldState  Field   { get; }
		IReadOnlyFigureState Figure  { get; }
		IReadOnlySpeedState  Speed   { get; }
		IReadOnlyRecordState Records { get; }

		InputState Input        { get; }
		int        Scores       { get; }
		int        ClearedLines { get; }
		bool       Finished     { get; }
		int        FitCount     { get; }
	}
}