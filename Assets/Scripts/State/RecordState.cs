using System.Collections.Generic;

namespace TetrisGame.State {
	public sealed class RecordState : IReadOnlyRecordState {
		public List<int> Records { get; } = new List<int>();

		IReadOnlyList<int> IReadOnlyRecordState.Records => Records;
	}
}