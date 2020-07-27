using System;
using UnityEngine;

namespace TetrisGame {
	public sealed class FigureManager : MonoBehaviour {
		[NonSerialized]
		public Figure CurrentFigure;
	}
}