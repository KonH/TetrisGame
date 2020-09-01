using System;
using System.Collections.Generic;

namespace TetrisGame.Service {
	[Serializable]
	sealed class SnapshotQueue {
		public List<SnapshotEntry> Queue = new List<SnapshotEntry>();
	}
}