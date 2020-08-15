namespace TetrisGame.State {
	public sealed class FieldState : IReadOnlyFieldState {
		public bool    IsDirty { get; set; }
		public int     Width   { get; }
		public int     Height  { get; }
		public bool[,] Field   { get; }

		public FieldState(int width, int height) {
			Width  = width;
			Height = height;
			Field  = new bool[Width, Height];
		}

		public bool GetState(int x, int y) {
			return Field[x, y];
		}
	}
}