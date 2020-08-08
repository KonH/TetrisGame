using TetrisGame.Entity;
using UnityEngine;

namespace TetrisGame.Logic {
	public struct FigureState {
		public bool         IsPresent;
		public int          Index;
		public FigureEntity Entity;
		public Vector2Int[] Positions;
		public float        TotalFall;
	}
}