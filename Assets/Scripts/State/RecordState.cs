using System.Collections.Generic;

namespace TetrisGame.State {
	public sealed class RecordState : IReadOnlyRecordState {
		public List<RecordUnit> Records { get; } = new List<RecordUnit>();

		IReadOnlyList<RecordUnit> IReadOnlyRecordState.Records => Records;
	}
}