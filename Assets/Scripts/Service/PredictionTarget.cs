namespace TetrisGame.Service {
	public sealed class PredictionTarget {
		public readonly int Offset;
		public readonly int Rotation;

		public PredictionTarget(int offset, int rotation) {
			Offset   = offset;
			Rotation = rotation;
		}
	}
}