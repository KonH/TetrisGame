namespace TetrisGame.State {
	public sealed class SpeedState {
		public int   CurrentLines;
		public float Current;
		public int   Level;

		public SpeedState(float initialSpeed) {
			Current = initialSpeed;
		}
	}
}