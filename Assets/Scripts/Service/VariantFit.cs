using TetrisGame.State;

namespace TetrisGame.Service {
	readonly struct VariantFit {
		public readonly InputState[] Inputs;
		public readonly GameState    State;
		public readonly float        Fit;

		public VariantFit(InputState[] inputs, GameState state, float fit) {
			Inputs = inputs;
			State  = state;
			Fit    = fit;
		}
	}
}