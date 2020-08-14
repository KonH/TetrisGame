using System.Collections.Generic;
using UnityEngine;

namespace TetrisGame.State {
	public sealed class FigureState {
		public Vector2       Origin   { get; set; }
		public List<Vector2> Elements { get; } = new List<Vector2>();

		public bool IsPresent => Elements.Count > 0;

		public void Reset() {
			Origin = Vector2.zero;
			Elements.Clear();
		}
	}
}