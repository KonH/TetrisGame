using System.Collections.Generic;
using UnityEngine;

namespace TetrisGame.State {
	public interface IReadOnlyFigureState {
		Vector2                Origin   { get; }
		IReadOnlyList<Vector2> Elements { get; }
	}
}