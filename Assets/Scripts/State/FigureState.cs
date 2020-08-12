using System.Collections.Generic;
using UnityEngine;

namespace TetrisGame.State {
	public sealed class FigureState {
		public Vector2       Origin   { get; set; }
		public List<Vector2> Elements { get; } = new List<Vector2>();
	}
}