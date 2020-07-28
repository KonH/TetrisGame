using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame {
	public sealed class FigureStateManager : MonoBehaviour {
		[SerializeField]
		FieldManager _field;

		[SerializeField]
		GameplayManager _gameplayManager;

		[SerializeField]
		ActiveFigureManager _activeFigureManager;

		[SerializeField]
		ElementManager _elementManager;

		[SerializeField]
		CollisionManager _collisionManager;

		void OnValidate() {
			Assert.IsNotNull(_field, nameof(_field));
			Assert.IsNotNull(_gameplayManager, nameof(_gameplayManager));
			Assert.IsNotNull(_activeFigureManager, nameof(_activeFigureManager));
			Assert.IsNotNull(_elementManager, nameof(_elementManager));
			Assert.IsNotNull(_collisionManager, nameof(_collisionManager));
		}

		void Update() {
			var active = _activeFigureManager.Active;
			if ( !active ) {
				return;
			}
			if ( IsFinished(active) ) {
				_gameplayManager.FinishFigure();
				return;
			}
			active.ApplyMove();
		}

		bool IsFinished(Figure figure) {
			return
				_field.IsOnBottom(figure) ||
				_collisionManager.HasPredictedCollisions(figure, _elementManager.Positions);

		}
	}
}