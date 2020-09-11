using System;

namespace TetrisGame.Service {
	[Serializable]
	public struct GeneticSettings {
		public float LinesCleared;
		public float WeightedHeight;
		public float CumulativeHeight;
		public float RelativeHeight;
		public float Holes;
		public float Roughness;
		public bool  UseDebugging;

		public GeneticSettings(
			float linesCleared, float weightedHeight, float cumulativeHeight, float relativeHeight, float holes, float roughness, bool useDebugging) {
			LinesCleared     = linesCleared;
			WeightedHeight   = weightedHeight;
			CumulativeHeight = cumulativeHeight;
			RelativeHeight   = relativeHeight;
			Holes            = holes;
			Roughness        = roughness;
			UseDebugging     = useDebugging;
		}
	}
}