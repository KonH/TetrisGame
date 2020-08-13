using NUnit.Framework;
using TetrisGame.Service;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Tests {
	public sealed class CollisionDetectorTest {
		[Test]
		public void HasDiscreteCollisions() {
			var field = new FieldState(2, 2);
			field.Field[0, 0] = true;
			var figure = new FigureState();
			figure.Elements.Add(new Vector2(0, 0));
			var detector = new CollisionDetector();

			var result = detector.HasCollisions(field, figure);

			Assert.IsTrue(result);
		}

		[Test]
		public void HasNoDiscreteCollisions() {
			var field = new FieldState(2, 2);
			var figure = new FigureState();
			figure.Elements.Add(new Vector2(0, 0));
			var detector = new CollisionDetector();

			var result = detector.HasCollisions(field, figure);

			Assert.IsFalse(result);
		}

		[Test]
		public void HasFuzzyCollisions() {
			var field = new FieldState(2, 2);
			field.Field[0, 0] = true;
			var figure = new FigureState();
			figure.Elements.Add(new Vector2(0.6f, 0.6f));
			var detector = new CollisionDetector();

			var result = detector.HasCollisions(field, figure);

			Assert.IsTrue(result);
		}
	}
}