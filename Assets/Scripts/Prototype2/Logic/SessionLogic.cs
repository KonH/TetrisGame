using System.Collections.Generic;
using TetrisGame.Entity;
using UnityEngine;

namespace TetrisGame.Logic {
	public sealed class SessionLogic {
		readonly FigurePool          _figurePool;
		readonly FigureSpawner       _figureSpawner;
		readonly SpeedCalculator     _speedCalculator;
		readonly FigureMover         _mover;
		readonly FigureDeconstructor _deconstructor;
		readonly FieldPresenter      _fieldPresenter;

		SessionState _state;

		public SessionLogic(
			int width, int height,
			IReadOnlyList<FigureEntity> figures, Transform figureParent,
			GameObject element, Transform elementParent,
			float initialSpeed) {
			_state = new SessionState {
				Field = new bool[width, height]
			};
			_figurePool      = new FigurePool(figures.Count);
			_figureSpawner   = new FigureSpawner(_figurePool, figures, figureParent);
			_speedCalculator = new SpeedCalculator(initialSpeed);
			_mover           = new FigureMover();
			_deconstructor   = new FigureDeconstructor();
			_fieldPresenter  = new FieldPresenter(width, height, new ElementPool(element, elementParent));
		}

		public void Update(float dt) {
			if ( !_state.Figure.IsPresent ) {
				var newFigure = _figureSpawner.Spawn();
				_state.Figure = newFigure;
			}
			var fall = _speedCalculator.GetCurrentFall(dt);
			if ( !_mover.TryApplyMovement(fall, _state.Field, ref _state.Figure, _state.Controls) ) {
				_deconstructor.Apply(_state.Field, _state.Figure.Positions);
				_fieldPresenter.Present(_state.Field);
				_figurePool.Recycle(_state.Figure.Index, _state.Figure.Entity);
				_state.Figure = default;
			}
			ResetControls();
		}

		public void SetupMoveInput(Vector2Int direction) {
			_state.Controls.MoveInput = direction;
		}

		public void SetupRotation(bool triggered) {
			_state.Controls.Rotate = triggered;
		}

		void ResetControls() {
			SetupMoveInput(Vector2Int.zero);
			SetupRotation(false);
		}
	}
}