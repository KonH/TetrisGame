using System.Collections.Generic;
using UnityEngine;

namespace TetrisGame.State {
	public sealed class FigureState : IReadOnlyFigureState {
		readonly IReadOnlyList<Vector2Int> _readOnlyElements;

		public Vector2          Origin   { get; set; }
		public List<Vector2Int> Elements { get; } = new List<Vector2Int>();

		public bool IsPresent => Elements.Count > 0;

		IReadOnlyList<Vector2Int> IReadOnlyFigureState.Elements => _readOnlyElements;

		public FigureState() {
			_readOnlyElements = Elements;
		}

		public void Reset() {
			Origin = Vector2.zero;
			Elements.Clear();
		}

		internal void Clone(IReadOnlyFigureState other) {
			Reset();
			Elements.AddRange(other.Elements);
			Origin = other.Origin;
		}
	}
}