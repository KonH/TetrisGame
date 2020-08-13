using System.Collections.Generic;
using NUnit.Framework;
using TetrisGame.Service;
using TetrisGame.State;

namespace TetrisGame.Tests {
	public sealed class LineDropperTest {
		[Test]
		public void IsLinesDropped() {
			var field = new FieldState(2, 4);
			field.Field[0, 2] = true;
			field.Field[0, 3] = true;
			var dropper = new LineDropper();

			dropper.Drop(field, new List<int> { 0, 1 });

			Assert.IsTrue(field.IsDirty);
			Assert.IsTrue(field.Field[0, 0]);
			Assert.IsTrue(field.Field[0, 1]);
			Assert.IsFalse(field.Field[0, 2]);
			Assert.IsFalse(field.Field[0, 3]);
		}
	}
}