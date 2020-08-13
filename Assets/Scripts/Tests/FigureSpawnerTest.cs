using NUnit.Framework;
using TetrisGame.Service;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Tests {
	public sealed class FigureSpawnerTest {
		[Test]
		public void IsSpawnedAtTopCenter() {
			var figure  = new FigureState();
			var spawner = new FigureSpawner(10, 20, new[] { new[] { new Vector2(0, 0) } });

			spawner.Spawn(figure);

			Assert.AreEqual(new Vector2(5, 20), figure.Origin);
		}

		[Test]
		public void IsSpawnCorrectElements() {
			var figure  = new FigureState();
			var spawner = new FigureSpawner(10, 20, new[] { new[] { new Vector2(0, 0) } });

			spawner.Spawn(figure);

			Assert.AreEqual(1, figure.Elements.Count);
			Assert.AreEqual(new Vector2(0, 0), figure.Elements[0]);
		}
	}
}