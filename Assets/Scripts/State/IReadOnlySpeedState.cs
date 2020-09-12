namespace TetrisGame.State {
	public interface IReadOnlySpeedState {
		int   CurrentLines { get; }
		int   Level        { get; }
		float Current      { get; }
	}
}