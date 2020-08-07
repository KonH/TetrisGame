using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame {
	public sealed class EndGameManager : MonoBehaviour {
		[SerializeField]
		ActiveFigureManager _activeFigure;

		[SerializeField]
		ElementManager _elementManager;

		[SerializeField]
		CollisionManager _collisionManager;

		void Awake() {
			Assert.IsNotNull(_activeFigure, nameof(_activeFigure));
			Assert.IsNotNull(_elementManager, nameof(_elementManager));
			Assert.IsNotNull(_collisionManager, nameof(_collisionManager));
		}

		public bool IsGameFinished() {
			var active = _activeFigure.Active;
			if ( !active ) {
				return false;
			}
			return _collisionManager.HasPredictedCollisions(active, _elementManager.Positions);
		}
	}
}