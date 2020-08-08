using TetrisGame.Logic;
using TetrisGame.Settings;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace TetrisGame.Wrapper {
	public sealed class SessionWrapper : MonoBehaviour {
		[SerializeField]
		SessionSettings _settings;

		[SerializeField]
		Transform _figureParent;

		[SerializeField]
		Transform _elementParent;

		SessionLogic _logic;

		void OnValidate() {
			Assert.IsNotNull(_settings, nameof(_settings));
			Assert.IsNotNull(_figureParent, nameof(_figureParent));
			Assert.IsNotNull(_elementParent, nameof(_elementParent));
		}

		void Awake() {
			_logic = new SessionLogic(
				_settings.Width, _settings.Height,
				_settings.Figures, _figureParent,
				_settings.Element, _elementParent,
				_settings.InitialSpeed);
		}

		void Update() {
			_logic.Update(Time.deltaTime);
		}

		public void MoveLeft(InputAction.CallbackContext ctx) {
			if ( ctx.started ) {
				_logic.SetupMoveInput(Vector2Int.left);
			}
		}

		public void MoveRight(InputAction.CallbackContext ctx) {
			if ( ctx.started ) {
				_logic.SetupMoveInput(Vector2Int.right);
			}
		}

		public void Rotate(InputAction.CallbackContext ctx) {
			if ( ctx.started ) {
				_logic.SetupRotation(true);
			}
		}

		public void SpeedUp(InputAction.CallbackContext ctx) {
			if ( ctx.started ) {
				_logic.SetupMoveInput(Vector2Int.down);
			}
		}
	}
}