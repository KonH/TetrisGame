using NUnit.Framework;
using TetrisGame.Service;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Tests {
	public sealed class LimitDetectorTest {
		[Test]
		public void IsNotOnBottom() {
			var figure = new FigureState {
				Origin = Vector2.up,
				Elements = {
					new Vector2Int(0, 0)
				}
			};
			var detector = new LimitDetector(2);

			var result = detector.IsLimitReached(figure);

			Assert.IsFalse(result);
		}

		[Test]
		public void IsOnBottomByOrigin() {
			var figure = new FigureState {
				Origin = Vector2.down * 2,
				Elements = {
					new Vector2Int(0, 1)
				}
			};
			var detector = new LimitDetector(2);

			var result = detector.IsLimitReached(figure);

			Assert.IsTrue(result);
		}

		[Test]
		public void IsOnBottomByElement() {
			var figure = new FigureState {
				Origin = Vector2.zero,
				Elements = {
					new Vector2Int(0, 0)
				}
			};
			var detector = new LimitDetector(2);

			var result = detector.IsLimitReached(figure);

			Assert.IsTrue(result);
		}

		[Test]
		public void IsOnLeftByOrigin() {
			var figure = new FigureState {
				Origin = Vector2.left * 2,
				Elements = {
					new Vector2Int(1, 1)
				}
			};
			var detector = new LimitDetector(2);

			var result = detector.IsLimitReached(figure);

			Assert.IsTrue(result);
		}

		[Test]
		public void IsOnLeftByElement() {
			var figure = new FigureState {
				Origin = Vector2.zero,
				Elements = {
					new Vector2Int(-1, 1)
				}
			};
			var detector = new LimitDetector(2);

			var result = detector.IsLimitReached(figure);

			Assert.IsTrue(result);
		}

		[Test]
		public void IsOnRightByOrigin() {
			var figure = new FigureState {
				Origin = Vector2.right * 3,
				Elements = {
					new Vector2Int(-1, 1)
				}
			};
			var detector = new LimitDetector(2);

			var result = detector.IsLimitReached(figure);

			Assert.IsTrue(result);
		}

		[Test]
		public void IsOnRightByElement() {
			var figure = new FigureState {
				Origin = Vector2.zero,
				Elements = {
					new Vector2Int(2, 1)
				}
			};
			var detector = new LimitDetector(2);

			var result = detector.IsLimitReached(figure);

			Assert.IsTrue(result);
		}
	}
}