using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	public sealed class GameLoop {
		readonly FigureSpawner _spawner;
		readonly FigureMover   _mover;
		readonly FigureRotator _rotator;
		readonly GameState     _state;

		public GameLoop(int width, int height, Vector2[][] figures, GameState state) {
			_spawner = new FigureSpawner(width, height, figures);
			_mover   = new FigureMover();
			_rotator = new FigureRotator();
			_state   = state;
		}

		public void Update(float dt) {
			MoveByInput();
			TrySpawnNewFigure();
			ResetInput();
		}

		void TrySpawnNewFigure() {
			if ( !_state.Figure.IsPresent ) {
				_spawner.Spawn(_state.Figure);
			}
		}

		void MoveByInput() {
			switch ( _state.Input ) {
				case InputState.MoveLeft:
					_mover.Move(_state.Figure, Vector2.left);
					break;

				case InputState.MoveRight:
					_mover.Move(_state.Figure, Vector2.right);
					break;

				case InputState.SpeedUp:
					_mover.Move(_state.Figure, Vector2.down);
					break;

				case InputState.Rotate:
					_rotator.Rotate(_state.Figure);
					break;
			}
		}

		void ResetInput() {
			_state.Input = InputState.None;
		}
	}
}