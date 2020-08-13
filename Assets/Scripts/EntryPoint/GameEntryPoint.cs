using TetrisGame.Presenter;
using TetrisGame.Settings;
using TetrisGame.State;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame.EntryPoint {
	public sealed class GameEntryPoint : MonoBehaviour {
		[SerializeField]
		GameGlobalSettings _globalSettings;

		[SerializeField]
		GameSceneSettings _sceneSettings;

		GameState       _state;
		FieldPresenter  _fieldPresenter;
		FigurePresenter _figurePresenter;

		void OnValidate() {
			Assert.IsNotNull(_globalSettings, nameof(_globalSettings));
			_sceneSettings.Validate();
		}

		void Awake() {
			_state = new GameState(_globalSettings.Width, _globalSettings.Height);
			var pool = new ElementPool(_globalSettings.ElementPrefab);
			_fieldPresenter = new FieldPresenter(
				pool, _sceneSettings.ElementRoot, _globalSettings.Width, _globalSettings.Height);
			_figurePresenter = new FigurePresenter(pool, _sceneSettings.FigureRoot);
		}

		void Update() {
			_fieldPresenter.Draw(_state.Field);
			_figurePresenter.Draw(_state.Figure);
		}
	}
}