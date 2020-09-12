using TetrisGame.State;

namespace TetrisGame.Service {
	sealed class GameStatePool : Pool<GameState> {
		readonly int   _width;
		readonly int   _height;
		readonly float _initialSpeed;

		public GameStatePool(int width, int height, float initialSpeed) {
			_width        = width;
			_height       = height;
			_initialSpeed = initialSpeed;
		}

		public GameState Clone(IReadOnlyGameState oldState) {
			var state = GetOrCreate();
			state.Clone(oldState);
			return state;
		}

		GameState GetOrCreate() {
			if ( Elements.Count > 0 ) {
				return Elements.Dequeue();
			}
			return new GameState(_width, _height, _initialSpeed);
		}
	}
}