namespace TetrisGame.State {
	public sealed class FieldState {
		public readonly int     Width;
		public readonly int     Height;
		public readonly bool[,] Field;

		public bool IsDirty;

		public FieldState(int width, int height) {
			Width  = width;
			Height = height;
			Field  = new bool[Width, Height];
		}
	}
}