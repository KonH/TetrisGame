using NUnit.Framework;
using TetrisGame.Service;
using TetrisGame.State;
using Assert = UnityEngine.Assertions.Assert;

namespace TetrisGame.Tests {
	public sealed class ScoreProducerTest {
		[Test]
		public void IsScoresNotAdded() {
			var state    = new GameState(1, 1, 0);
			var producer = new ScoreProducer(new[] { 0, 100 });

			producer.AddScores(state, 0);

			Assert.AreEqual(0, state.Scores);
		}

		[Test]
		public void IsScoresAdded() {
			var state    = new GameState(1, 1, 0);
			var producer = new ScoreProducer(new[] { 0, 100 });

			producer.AddScores(state, 1);

			Assert.AreEqual(100, state.Scores);
		}

		[Test]
		public void IsScoresClamped() {
			var state    = new GameState(1, 1, 0);
			var producer = new ScoreProducer(new[] { 0, 100 });

			producer.AddScores(state, 2);

			Assert.AreEqual(100, state.Scores);
		}
	}
}