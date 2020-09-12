using NUnit.Framework;
using TetrisGame.Service;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Tests {
	public sealed class FigureSpawnerTest {
		[Test]
		public void IsSpawnedAtTopCenter() {
			var figure  = new FigureState();
			var spawner = new FigureSpawner(10, 20, 42, new[] { new[] { new Vector2Int(0, 0) } });

			spawner.Spawn(figure);

			Assert.AreEqual(new Vector2(4, 19), figure.Origin);
		}

		[Test]
		public void IsSpawnCorrectElements() {
			var figure  = new FigureState();
			var spawner = new FigureSpawner(10, 20, 42, new[] { new[] { new Vector2Int(0, 0) } });

			spawner.Spawn(figure);

			Assert.AreEqual(1, figure.Elements.Count);
			Assert.AreEqual(new Vector2Int(0, 0), figure.Elements[0]);
		}
	}
}