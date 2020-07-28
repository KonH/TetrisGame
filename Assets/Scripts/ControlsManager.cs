using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace TetrisGame {
	public sealed class ControlsManager : MonoBehaviour {
		[SerializeField]
		ActiveFigureManager _figureManager;

		void OnValidate() {
			Assert.IsNotNull(_figureManager, nameof(_figureManager));
		}

		public void MoveLeft(InputAction.CallbackContext ctx) {
			if ( ctx.started ) {
				_figureManager.TryMove(Vector3.left);
			}
		}

		public void MoveRight(InputAction.CallbackContext ctx) {
			if ( ctx.started ) {
				_figureManager.TryMove(Vector3.right);
			}
		}

		public void Rotate(InputAction.CallbackContext ctx) {
			if ( ctx.started ) {
				_figureManager.TryRotate(90);
			}
		}

		public void SpeedUp(InputAction.CallbackContext ctx) {
			if ( ctx.started ) {
				_figureManager.TryMove(Vector3.down);
			}
		}
	}
}