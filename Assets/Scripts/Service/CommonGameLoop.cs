using System.Collections.Generic;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	/// <summary>
	/// Coordinates all game logic parts
	/// </summary>
	public sealed class CommonGameLoop : IGameLoop {
		GameState _state;

		readonly FigureSpawner       _spawner;
		readonly FigureMover         _mover;
		readonly FigureRotator       _rotator;
		readonly CollisionDetector   _collisionDetector;
		readonly LimitDetector       _limitDetector;
		readonly FigureDeconstructor _deconstructor;
		readonly SpeedController     _speed;
		readonly LineDetector        _lineDetector;
		readonly LineDropper         _lineDropper;
		readonly ScoreProducer       _scoreProducer;

		readonly List<int> _lines = new List<int>();

		public IReadOnlyGameState State => _state;

		public CommonGameLoop(GameLoopSettings settings, GameState state) {
			_state             = state;
			_spawner           = new FigureSpawner(settings.Width, settings.Height, settings.RandomSeed, settings.Figures);
			_mover             = new FigureMover();
			_rotator           = new FigureRotator();
			_collisionDetector = new CollisionDetector();
			_limitDetector     = new LimitDetector(settings.Width);
			_deconstructor     = new FigureDeconstructor();
			_speed             = new SpeedController(settings.LinesToIncrease, settings.IncreaseValue);
			_lineDetector      = new LineDetector();
			_lineDropper       = new LineDropper();
			_scoreProducer     = new ScoreProducer(settings.ScorePerLines);
		}

		internal void Reset(GameState state) {
			_state = state;
		}

		public void Update(float dt) {
			if ( PreUpdate() ) {
				PostUpdate(dt);
			}
		}

		internal bool PreUpdate() {
			ResetField();
			if ( !TrySpawnNewFigure() ) {
				ResetInput();
				return false;
			}
			return true;
		}

		internal void PostUpdate(float dt) {
			MoveByInput();
			MoveBySpeed(dt);
			ResetInput();
		}

		public void UpdateInput(InputState input) {
			_state.Input = input;
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
			_state.Figure.Reset();
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
			MoveVertical(_state.Speed.Current * dt);
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
			_state.Figure.Reset();
			ProcessLines();
			_state.FitCount++;
		}

		void ProcessLines() {
			_lineDetector.DetectLines(_state.Field, _lines);
			_lineDropper.Drop(_state.Field, _lines);
			_scoreProducer.AddScores(_state, _lines.Count);
			_speed.ApplyLines(_state.Speed, _lines.Count);
			_state.ClearedLines += _lines.Count;
		}

		bool ShouldDeconstruct() =>
			_collisionDetector.HasCollisions(_state.Field, _state.Figure) ||
			_limitDetector.IsLimitReached(_state.Figure);

		void ResetInput() {
			_state.Input = InputState.None;
		}
	}
}