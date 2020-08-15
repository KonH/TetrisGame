namespace TetrisGame.State {
	public interface IReadOnlyFieldState {
		bool IsDirty { get; }
		int  Width   { get; }
		int  Height  { get; }

		bool GetState(int x, int y);
	}
}