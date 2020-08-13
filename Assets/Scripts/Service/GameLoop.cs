using System.Collections.Generic;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	public sealed class GameLoop {
		readonly FigureSpawner       _spawner;
		readonly FigureMover         _mover;
		readonly FigureRotator       _rotator;
		readonly CollisionDetector   _collisionDetector;
		readonly LimitDetector       _limitDetector;
		readonly FigureDeconstructor _deconstructor;
		readonly SpeedController     _speed;
		readonly LineDetector        _lineDetector;
		readonly LineDropper         _lineDropper;
		readonly GameState           _state;

		readonly List<int> _lines = new List<int>();

		public GameLoop(int width, int height, float initialSpeed, Vector2[][] figures, GameState state) {
			_spawner           = new FigureSpawner(width, height, figures);
			_mover             = new FigureMover();
			_rotator           = new FigureRotator();
			_collisionDetector = new CollisionDetector();
			_limitDetector     = new LimitDetector(width);
			_deconstructor     = new FigureDeconstructor();
			_speed             = new SpeedController(initialSpeed);
			_lineDetector      = new LineDetector();
			_lineDropper       = new LineDropper();
			_state             = state;
		}

		public void Update(float dt) {
			ResetField();
			if ( !TrySpawnNewFigure() ) {
				ResetInput();
				return;
			}
			MoveByInput();
			MoveBySpeed(dt);
			ProcessLines();
			ResetInput();
		}

		void ResetField() {
			_state.Field.IsDirty = false;
		}

		bool TrySpawnNewFigure() {
			if ( _state.Figure.IsPresent ) {
				return true;
			}
			_spawner.Spawn(_state.Figure);
			if ( !ShouldDeconstruct() ) {
				return true;
			}
			_state.Figure.Elements.Clear();
			_state.Finished = true;
			return false;
		}

		void MoveByInput() {
			switch ( _state.Input ) {
				case InputState.MoveLeft:
					MoveHorizontal(false);
					break;

				case InputState.MoveRight:
					MoveHorizontal(true);
					break;

				case InputState.SpeedUp:
					MoveVertical();
					break;

				case InputState.Rotate:
					_rotator.Rotate(_state.Figure);
					if ( ShouldDeconstruct() ) {
						_rotator.RotateBack(_state.Figure);
					}
					break;
			}
		}

		void MoveBySpeed(float dt) {
			MoveVertical(_speed.Speed * dt);
		}

		void ProcessLines() {
			_lineDetector.DetectLines(_state.Field, _lines);
			_lineDropper.Drop(_state.Field, _lines);
		}

		void MoveHorizontal(bool right) {
			var step = right ? Vector2.right : Vector2.left;
			_mover.Move(_state.Figure, step);
			if ( ShouldDeconstruct() ) {
				_mover.Move(_state.Figure, -step);
			}
		}

		void MoveVertical(float distance = 1.0f) {
			var step = Vector2.down * distance;
			_mover.Move(_state.Figure, step);
			if ( ShouldDeconstruct() ) {
				_mover.Move(_state.Figure, -step);
				Deconstruct();
			}
		}

		void Deconstruct() {
			_deconstructor.Place(_state.Field, _state.Figure);
			_state.Figure.Elements.Clear();
		}

		bool ShouldDeconstruct() =>
			_collisionDetector.HasCollisions(_state.Field, _state.Figure) ||
			_limitDetector.IsLimitReached(_state.Figure);

		void ResetInput() {
			_state.Input = InputState.None;
		}
	}
}