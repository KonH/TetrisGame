namespace TetrisGame.State {
	public sealed class SpeedState : IReadOnlySpeedState {
		public int   CurrentLines { get; set; }
		public float Current      { get; set; }
		public int   Level        { get; set; }

		public SpeedState(float initialSpeed) {
			Current = initialSpeed;
		}

		internal void Clone(IReadOnlySpeedState other) {
			CurrentLines = other.CurrentLines;
			Current      = other.Current;
			Level        = other.Level;
		}
	}
}