using System.Collections.Generic;
using NUnit.Framework;
using TetrisGame.Service;
using TetrisGame.State;

namespace TetrisGame.Tests {
	public sealed class LineDetectorTest {
		[Test]
		public void NoLineDetected() {
			var field = new FieldState(2, 2);
			field.Field[0, 0] = true;
			var detector = new LineDetector();
			var result = new List<int>();

			detector.DetectLines(field, result);

			Assert.IsEmpty(result);
		}

		[Test]
		public void IsLineDetected() {
			var field = new FieldState(2, 2);
			field.Field[0, 0] = true;
			field.Field[1, 0] = true;
			var detector = new LineDetector();
			var result   = new List<int>();

			detector.DetectLines(field, result);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(0, result[0]);
		}
	}
}