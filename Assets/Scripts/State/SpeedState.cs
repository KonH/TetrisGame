namespace TetrisGame.State {
	public sealed class SpeedState : IReadOnlySpeedState {
		public int   CurrentLines;
		public float Current;

		public int   Level { get; set; }

		public SpeedState(float initialSpeed) {
			Current = initialSpeed;
		}
	}
}