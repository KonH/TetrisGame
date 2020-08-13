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
			figure.Elements.Add(new Vector2(0, 1));
			var rotator = new FigureRotator();

			rotator.Rotate(figure);

			var element = figure.Elements[0];
			Assert.True(Mathf.Approximately(1.0f, element.x), element.ToString());
			Assert.True(Mathf.Approximately(0.0f, element.y), element.ToString());
		}
	}
}