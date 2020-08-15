namespace TetrisGame.State {
	public interface IReadOnlyGameState {
		IReadOnlyFieldState  Field  { get; }
		IReadOnlyFigureState Figure { get; }
		IReadOnlySpeedState  Speed  { get; }

		int  Scores   { get; }
		bool Finished { get; }
	}
}