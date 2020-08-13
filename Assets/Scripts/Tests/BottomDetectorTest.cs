using NUnit.Framework;
using TetrisGame.Service;
using TetrisGame.State;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace TetrisGame.Tests {
	public sealed class BottomDetectorTest {
		[Test]
		public void IsNotOnBottom() {
			var figure = new FigureState {
				Origin = Vector2.up,
				Elements = {
					new Vector2(0, 0)
				}
			};
			var detector = new BottomDetector();

			var result = detector.IsOnBottom(figure);

			Assert.IsFalse(result);
		}

		[Test]
		public void IsOnBottomByOrigin() {
			var figure = new FigureState {
				Origin = Vector2.down * 2,
				Elements = {
					new Vector2(0, 1)
				}
			};
			var detector = new BottomDetector();

			var result = detector.IsOnBottom(figure);

			Assert.IsTrue(result);
		}

		[Test]
		public void IsOnBottomByElement() {
			var figure = new FigureState {
				Origin = Vector2.zero,
				Elements = {
					new Vector2(0, 0)
				}
			};
			var detector = new BottomDetector();

			var result = detector.IsOnBottom(figure);

			Assert.IsTrue(result);
		}
	}
}