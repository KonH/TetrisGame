namespace TetrisGame.Service {
	public sealed class SpeedController {
		readonly float _initialSpeed;

		public SpeedController(float initialSpeed) {
			_initialSpeed = initialSpeed;
		}

		public float Speed => _initialSpeed;
	}
}