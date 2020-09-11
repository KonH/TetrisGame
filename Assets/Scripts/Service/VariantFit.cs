using TetrisGame.State;

namespace TetrisGame.Service {
	readonly struct VariantFit {
		public readonly InputState[]       Inputs;
		public readonly IReadOnlyGameState State;
		public readonly float              Fit;

		public VariantFit(InputState[] inputs, IReadOnlyGameState state, float fit) {
			Inputs = inputs;
			State  = state;
			Fit    = fit;
		}
	}
}