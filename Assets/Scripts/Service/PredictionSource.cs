using UnityEngine;

namespace TetrisGame.Service {
	public struct PredictionSource {
		public readonly Vector2[]    Elements;
		public readonly Vector2Int[] Field;

		public PredictionSource(Vector2[] elements, Vector2Int[] field) {
			Elements = elements;
			Field    = field;
		}
	}
}