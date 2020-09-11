using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	/// <summary>
	/// Create new random figure
	/// </summary>
	public sealed class FigureSpawner {
		readonly int         _width;
		readonly int         _height;
		readonly Vector2[][] _figures;

		readonly System.Random _random;

		public FigureSpawner(int width, int height, int randomSeed, Vector2[][] figures) {
			_width   = width;
			_height  = height;
			_figures = figures;
			_random = new System.Random(randomSeed);
		}

		public void Spawn(FigureState figure) {
			// ReSharper disable once PossibleLossOfFraction
			// (we need discrete start position)
			figure.Reset();
			figure.Origin = new Vector2(_width / 2 - 1, _height - 1);
			var elements = _figures[_random.Next(0, _figures.Length)];
			figure.Elements.AddRange(elements);
		}
	}
}