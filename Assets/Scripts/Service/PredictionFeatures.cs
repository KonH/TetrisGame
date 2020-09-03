namespace TetrisGame.Service {
	public struct PredictionFeatures {
		public readonly int     FigureType;
		public readonly float[] NormalizedTopLine;

		public PredictionFeatures(int figureType, float[] normalizedTopLine) {
			FigureType        = figureType;
			NormalizedTopLine = normalizedTopLine;
		}
	}
}