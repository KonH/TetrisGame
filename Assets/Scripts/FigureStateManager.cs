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

		void OnValidate() {
			Assert.IsNotNull(_field, nameof(_field));
			Assert.IsNotNull(_gameplayManager, nameof(_gameplayManager));
			Assert.IsNotNull(_activeFigureManager, nameof(_activeFigureManager));
		}

		void Update() {
			var active = _activeFigureManager.Active;
			if ( !active ) {
				return;
			}
			if ( !IsFinished(active) ) {
				return;
			}
			_gameplayManager.FinishFigure();
		}

		bool IsFinished(Figure figure) {
			return _field.IsOnBottom(figure);
		}
	}
}