namespace TetrisGame.Logic {
	public sealed class SpeedCalculator {
		readonly float _initialSpeed;

		public SpeedCalculator(float initialSpeed) {
			_initialSpeed = initialSpeed;
		}

		public float GetCurrentFall(float dt) {
			return _initialSpeed * dt;
		}
	}
}