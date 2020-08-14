using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	public sealed class FigureSpawner {
		readonly int         _width;
		readonly int         _height;
		readonly Vector2[][] _figures;

		public FigureSpawner(int width, int height, Vector2[][] figures) {
			_width   = width;
			_height  = height;
			_figures = figures;
		}

		public void Spawn(FigureState figure) {
			// ReSharper disable once PossibleLossOfFraction
			// (we need discrete start position)
			figure.Reset();
			figure.Origin = new Vector2(_width / 2 - 1, _height - 1);
			var elements = _figures[Random.Range(0, _figures.Length)];
			figure.Elements.AddRange(elements);
		}
	}
}