using NUnit.Framework;
using TetrisGame.Service;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Tests {
	public sealed class FigureMoverTest {
		[Test]
		public void IsMoved() {
			var figure = new FigureState();
			var mover  = new FigureMover();

			mover.Move(figure, Vector2.down);

			Assert.AreEqual(-1.0f, figure.Origin.y);
		}
	}
}