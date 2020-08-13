using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	public sealed class GameLoop {
		readonly FigureSpawner _spawner;
		readonly GameState     _state;

		public GameLoop(int width, int height, Vector2[][] figures, GameState state) {
			_spawner = new FigureSpawner(width, height, figures);
			_state = state;
		}

		public void Update(float dt) {
			TrySpawnNewFigure();
		}

		void TrySpawnNewFigure() {
			if ( !_state.Figure.IsPresent ) {
				_spawner.Spawn(_state.Figure);
			}
		}
	}
}