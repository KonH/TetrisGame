using System.Collections.Generic;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	/// <summary>
	/// Coordinates all game logic parts
	/// </summary>
	public sealed class GameLoop {
		readonly GameState           _state;
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
		readonly RecordWriter        _recordWriter;
		readonly SnapshotMaker       _snapshotMaker;

		readonly List<int> _lines = new List<int>();

		public IReadOnlyGameState State => _state;

		public GameLoop(
			int width, int height, float initialSpeed, int linesToIncrease, float increaseValue,
			Vector2[][] figures, IReadOnlyList<int> scorePerLines) {
			_state             = new GameState(width, height, initialSpeed);
			_spawner           = new FigureSpawner(width, height, figures);
			_mover             = new FigureMover();
			_rotator           = new FigureRotator();
			_collisionDetector = new CollisionDetector();
			_limitDetector     = new LimitDetector(width);
			_deconstructor     = new FigureDeconstructor();
			_speed             = new SpeedController(linesToIncrease, increaseValue);
			_lineDetector      = new LineDetector();
			_lineDropper       = new LineDropper();
			_scoreProducer     = new ScoreProducer(scorePerLines);
			_recordWriter      = new RecordWriter();
			_snapshotMaker     = new SnapshotMaker(_state);

			var recordReader = new RecordReader();
			recordReader.Read(_state.Records);
		}

		public void Update(float dt) {
			ResetField();
			if ( !TrySpawnNewFigure() ) {
				ResetInput();
				return;
			}
			_snapshotMaker.TryTakeSnapshot();
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
			_recordWriter.Write(_state.Records, _state.Scores);
			_snapshotMaker.Save();
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
		}

		bool ShouldDeconstruct() =>
			_collisionDetector.HasCollisions(_state.Field, _state.Figure) ||
			_limitDetector.IsLimitReached(_state.Figure);

		void ResetInput() {
			_state.Input = InputState.None;
		}
	}
}