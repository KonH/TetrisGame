namespace TetrisGame.State {
	public sealed class FieldState : IReadOnlyFieldState {
		readonly int     _width;
		readonly int     _height;
		readonly bool[,] _field;

		public bool IsDirty { get; set; }

		public int     Width  => _width;
		public int     Height => _height;
		public bool[,] Field  => _field;

		public FieldState(int width, int height) {
			_width  = width;
			_height = height;
			_field  = new bool[Width, Height];
		}

		public bool GetState(int x, int y) {
			return (x >= 0) && (y >= 0) && (x < _width) && (y < _height) && _field[x, y];
		}

		public bool GetStateUnsafe(int x, int y) {
			return _field[x, y];
		}
	}
}