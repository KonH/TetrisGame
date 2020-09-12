using NUnit.Framework;
using TetrisGame.Service;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Tests {
	public sealed class FigureRotatorTest {
		[Test]
		public void IsRotated() {
			/*
			 * [ * - ]
			 * [ - - ]
			 * =>
			 * [ - - ]
			 * [ - * ]
			 */
			var figure = new FigureState();
			figure.Elements.Add(new Vector2Int(0, 1));
			var rotator = new FigureRotator();

			rotator.Rotate(figure);

			var element = figure.Elements[0];
			Assert.AreEqual(new Vector2Int(1, 0), element);
		}

		[Test]
		public void IsRotatedBack() {
			/*
			 * [ - - ]
			 * [ - * ]
			 * =>
			 * [ * - ]
			 * [ - - ]
			 * =>
			 */
			var figure = new FigureState();
			figure.Elements.Add(new Vector2Int(1, 0));
			var rotator = new FigureRotator();

			rotator.RotateBack(figure);

			var element = figure.Elements[0];
			Assert.AreEqual(new Vector2Int(0, 1), element);
		}
	}
}