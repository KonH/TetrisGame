using System.Collections.Generic;
using TetrisGame.State;

namespace TetrisGame.Service {
	public sealed class GeneticPlayer {
		readonly GeneticPredictor _predictor;

		readonly Queue<InputState> _inputs = new Queue<InputState>();

		int _prevFitCount = -1;

		public GeneticPlayer(GameLoopSettings loopSettings, GeneticSettings geneticSettings) {
			_predictor = new GeneticPredictor(loopSettings, geneticSettings);
		}

		public void Update(GameState state) {
			if ( _prevFitCount == state.FitCount ) {
				TryProcessInput(state);
				return;
			}
			RetrieveInput(state);
			_prevFitCount = state.FitCount;
		}

		void TryProcessInput(GameState state) {
			var input = (_inputs.Count > 0) ? _inputs.Dequeue() : InputState.SpeedUp;
			state.Input = input;
		}

		void RetrieveInput(GameState state) {
			var inputs = _predictor.GetBestInputs(state);
			_inputs.Clear();
			foreach ( var input in inputs ) {
				_inputs.Enqueue(input);
			}
		}
	}
}