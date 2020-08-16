namespace TetrisGame.State {
	public interface IReadOnlyGameState {
		IReadOnlyFieldState  Field   { get; }
		IReadOnlyFigureState Figure  { get; }
		IReadOnlySpeedState  Speed   { get; }
		IReadOnlyRecordState Records { get; }

		int  Scores   { get; }
		bool Finished { get; }
		int  FitCount { get; }
	}
}