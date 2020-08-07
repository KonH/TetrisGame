using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame {
	public sealed class FigureFallManager : MonoBehaviour {
		[SerializeField]
		float _initialFallSpeed;

		[SerializeField]
		ActiveFigureManager _figureManager;

		void OnValidate() {
			Assert.IsNotNull(_figureManager, nameof(_figureManager));
		}

		void Update() {
			var active = _figureManager.Active;
			if ( !active ) {
				return;
			}
			active.ScheduleMove(Vector3.down * (_initialFallSpeed * Time.deltaTime));
		}
	}
}