using System;
using System.Collections.Generic;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	[Serializable]
	struct SnapshotEntry {
		public InputState       InputState;
		public int              FitCount;
		public Vector2          Origin;
		public List<Vector2>    Figure;
		public List<Vector2Int> Field;
	}
}