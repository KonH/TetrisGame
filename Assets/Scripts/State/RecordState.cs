using System.Collections.Generic;

namespace TetrisGame.State {
	public sealed class RecordState : IReadOnlyRecordState {
		public List<RecordUnit> Records { get; } = new List<RecordUnit>();

		IReadOnlyList<RecordUnit> IReadOnlyRecordState.Records => Records;

		internal void Clone(IReadOnlyRecordState other) {
			Records.Clear();
			for ( var i = 0; i < other.Records.Count; i++ ) {
				Records.Add(other.Records[i]);
			}
		}
	}
}