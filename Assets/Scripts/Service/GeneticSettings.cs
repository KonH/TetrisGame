using System;

namespace TetrisGame.Service {
	[Serializable]
	public struct GeneticSettings {
		public int FillEntryCoeff;
		public int TopEntryCoeff;
		public int HoleEntryCoeff;

		public GeneticSettings(int fillEntryCoeff, int topEntryCoeff, int holeEntryCoeff) {
			FillEntryCoeff = fillEntryCoeff;
			TopEntryCoeff  = topEntryCoeff;
			HoleEntryCoeff = holeEntryCoeff;
		}

		public override string ToString() {
			return $"{nameof(FillEntryCoeff)}: {FillEntryCoeff}, " +
			       $"{nameof(TopEntryCoeff)}: {TopEntryCoeff}, " +
			       $"{nameof(HoleEntryCoeff)}: {HoleEntryCoeff}";
		}
	}
}