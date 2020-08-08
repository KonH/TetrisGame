using NUnit.Framework;
using TetrisGame.Logic;
using UnityEngine;

namespace TetrisGame.Tests {
	public sealed class MovementCheckerTest {
		readonly MovementChecker _checker = new MovementChecker();

		[Test]
		public void CantMoveOutsideField() {
			var  field  = new bool[3, 3];
			var  figure = new Vector2Int[1];
			bool allowed, finished;

			figure[0]           = new Vector2Int(0, 1);
			(allowed, finished) = _checker.CalculateAbility(field, figure, Vector2Int.left);
			Assert.IsFalse(allowed);
			Assert.IsFalse(finished);

			figure[0]           = new Vector2Int(2, 1);
			(allowed, finished) = _checker.CalculateAbility(field, figure, Vector2Int.right);
			Assert.IsFalse(allowed);
			Assert.IsFalse(finished);
		}

		[Test]
		public void CantMoveIntoBusySpace() {
			var field  = new bool[3, 3];
			field[0, 1] = true;
			field[2, 1] = true;
			var figure = new [] { new Vector2Int(1, 1) };
			bool allowed, finished;

			(allowed, finished) = _checker.CalculateAbility(field, figure, Vector2Int.left);
			Assert.IsFalse(allowed);
			Assert.IsFalse(finished);

			(allowed, finished) = _checker.CalculateAbility(field, figure, Vector2Int.right);
			Assert.IsFalse(allowed);
			Assert.IsFalse(finished);
		}

		[Test]
		public void CanMoveInsideField() {
			var field = new bool[3, 3];
			var  figure = new [] { new Vector2Int(1, 1) };
			bool allowed, finished;

			(allowed, finished) = _checker.CalculateAbility(field, figure, Vector2Int.left);
			Assert.IsTrue(allowed);
			Assert.IsFalse(finished);

			(allowed, finished) = _checker.CalculateAbility(field, figure, Vector2Int.right);
			Assert.IsTrue(allowed);
			Assert.IsFalse(finished);
		}

		[Test]
		public void MoveIsNotFinishedInFreeSpace() {
			var field  = new bool[3, 3];
			var figure = new [] { new Vector2Int(1, 2) };

			var (allowed, finished) = _checker.CalculateAbility(field, figure, Vector2Int.down);
			Assert.IsTrue(allowed);
			Assert.IsFalse(finished);
		}

		[Test]
		public void MoveIsFinishedOnBottom() {
			var field  = new bool[3, 3];
			var figure = new [] { new Vector2Int(1, 1) };

			var (allowed, finished) = _checker.CalculateAbility(field, figure, Vector2Int.down);
			Assert.IsTrue(allowed);
			Assert.IsTrue(finished);
		}

		[Test]
		public void MoveIsFinishedIntoBusySpace() {
			var field  = new bool[3, 3];
			field[1, 1] = true;
			var figure = new [] { new Vector2Int(1, 2) };

			var (allowed, finished) = _checker.CalculateAbility(field, figure, Vector2Int.down);
			Assert.IsFalse(allowed);
			Assert.IsTrue(finished);
		}
	}
}