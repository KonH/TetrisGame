using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace TetrisGame {
	public sealed class ControlsManager : MonoBehaviour {
		[SerializeField]
		ActiveFigureManager FigureManager;

		void OnValidate() {
			Assert.IsNotNull(FigureManager, nameof(FigureManager));
		}

		public void MoveLeft(InputAction.CallbackContext ctx) {
			if ( ctx.started ) {
				FigureManager.TryMove(Vector3.left);
			}
		}

		public void MoveRight(InputAction.CallbackContext ctx) {
			if ( ctx.started ) {
				FigureManager.TryMove(Vector3.right);
			}
		}

		public void Rotate(InputAction.CallbackContext ctx) {
			if ( ctx.started ) {
				FigureManager.TryRotate(90);
			}
		}

		public void SpeedUp(InputAction.CallbackContext ctx) {
			if ( ctx.started ) {
				FigureManager.TryMove(Vector3.down);
			}
		}
	}
}