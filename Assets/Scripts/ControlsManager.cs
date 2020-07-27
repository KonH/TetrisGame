using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace TetrisGame {
	public sealed class ControlsManager : MonoBehaviour {
		[SerializeField]
		FigureManager FigureManager;

		void OnValidate() {
			Assert.IsNotNull(FigureManager, nameof(FigureManager));
		}

		public void RotateLeft(InputAction.CallbackContext _) {
			TryRotate(-90);
		}

		public void RotateRight(InputAction.CallbackContext _) {
			TryRotate(90);
		}

		void TryRotate(float angle) {
			var figure = FigureManager.CurrentFigure;
			if ( !figure ) {
				return;
			}
			figure.Rotate(angle);
		}
	}
}