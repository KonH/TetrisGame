using NUnit.Framework;
using TetrisGame.Service;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Tests {
	public sealed class FigureDeconstructorTest {
		[Test]
		public void IsFigureSimplyDeconstructed() {
			var field  = new FieldState(1, 2);
			var figure = new FigureState();
			figure.Elements.Add(new Vector2Int(0, 0));
			var deconstructor = new FigureDeconstructor();

			deconstructor.Place(field, figure);

			Assert.IsTrue(field.IsDirty);
			Assert.IsTrue(field.Field[0, 0]);
			Assert.IsFalse(field.Field[0, 1]);
		}

		[Test]
		public void IsOriginRelatedDeconstructed() {
			var field  = new FieldState(1, 2);
			var figure = new FigureState();
			figure.Origin = new Vector2(0, 1);
			figure.Elements.Add(new Vector2Int(0, 0));
			var deconstructor = new FigureDeconstructor();

			deconstructor.Place(field, figure);

			Assert.IsTrue(field.IsDirty);
			Assert.IsFalse(field.Field[0, 0]);
			Assert.IsTrue(field.Field[0, 1]);
		}

		[Test]
		public void IsFigureRoundedDeconstructed() {
			var field  = new FieldState(1, 2);
			var figure = new FigureState();
			figure.Origin = new Vector2(-0.01f, -0.01f);
			figure.Elements.Add(new Vector2Int(1, 1));
			var deconstructor = new FigureDeconstructor();

			deconstructor.Place(field, figure);

			Assert.IsTrue(field.IsDirty);
			Assert.IsTrue(field.Field[0, 0]);
			Assert.IsFalse(field.Field[0, 1]);
		}

		[Test]
		public void IsFigureSkippedOutOfRange() {
			var field  = new FieldState(1, 2);
			var figure = new FigureState();
			figure.Elements.Add(new Vector2Int(2, 3));
			var deconstructor = new FigureDeconstructor();

			deconstructor.Place(field, figure);

			Assert.IsFalse(field.IsDirty);
			Assert.IsFalse(field.Field[0, 0]);
			Assert.IsFalse(field.Field[0, 1]);
		}
	}
}