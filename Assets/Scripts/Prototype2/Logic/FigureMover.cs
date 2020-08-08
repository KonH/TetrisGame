using UnityEngine;

namespace TetrisGame.Logic {
	public sealed class FigureMover {
		readonly MovementChecker _checker = new MovementChecker();

		public bool TryApplyMovement(
			float currentFall, bool[,] field, ref FigureState figure, ControlsState controls) {
			figure.TotalFall += currentFall;
			var isFallen = (figure.TotalFall >= 1);
			if ( isFallen ) {
				figure.TotalFall -= 1;
			}
			var shouldRecalculate = (figure.Positions == null) || isFallen;
			if ( controls.Rotate && !figure.Entity.FixedRotation ) {
				figure.Entity.transform.Rotate(Vector3.forward, 90);
				shouldRecalculate = true;
			}
			if ( shouldRecalculate ) {
				figure.Positions = CalculatePositions(figure);
			}
			var stateMoveInput = controls.MoveInput;
			if ( isFallen ) {
				stateMoveInput += Vector2Int.down;
			}
			var (allowed, finished) = _checker.CalculateAbility(field, figure.Positions, stateMoveInput);
			if ( allowed ) {
				ApplyStateMovement(figure, stateMoveInput);
				var presentMoveInput = new Vector2(controls.MoveInput.x, controls.MoveInput.y);
				if ( !finished ) {
					presentMoveInput.y -= currentFall;
				}
				ApplyPresenterMovement(figure, presentMoveInput);
			}
			if ( !finished ) {
				(_, finished) = _checker.CalculateAbility(field, figure.Positions, Vector2Int.down);
			}
			return !finished;
		}

		Vector2Int[] CalculatePositions(FigureState figure) {
			var entity = figure.Entity;
			var result = figure.Positions ?? new Vector2Int[entity.Elements.Count];
			for ( var i = 0; i < entity.Elements.Count; i++ ) {
				var position = entity.Elements[i].position;
				result[i] = new Vector2Int((int) position.x, (int) position.y);
			}
			return result;
		}

		void ApplyStateMovement(FigureState figure, Vector2Int moveInput) {
			for ( var i = 0; i < figure.Positions.Length; i++ ) {
				figure.Positions[i] = figure.Positions[i] + moveInput;
			}
		}

		void ApplyPresenterMovement(FigureState figure, Vector2 moveInput) {
			var moveVector = new Vector3(moveInput.x, moveInput.y);
			figure.Entity.transform.position += moveVector;
		}
	}
}