using TetrisGame.Presenter;
using TetrisGame.Service;
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
		GameLoop        _loop;
		FieldPresenter  _fieldPresenter;
		FigurePresenter _figurePresenter;

		void OnValidate() {
			Assert.IsNotNull(_globalSettings, nameof(_globalSettings));
			_sceneSettings.Validate();
		}

		void Awake() {
			_state = new GameState(_globalSettings.Width, _globalSettings.Height);
			_loop = new GameLoop(_globalSettings.Width, _globalSettings.Height, PopulateFigures(), _state);
			var pool = new ElementPool(_globalSettings.ElementPrefab);
			_fieldPresenter = new FieldPresenter(
				pool, _sceneSettings.ElementRoot, _globalSettings.Width, _globalSettings.Height);
			_figurePresenter = new FigurePresenter(pool, _sceneSettings.FigureRoot);
		}

		Vector2[][] PopulateFigures() {
			var settings = _globalSettings.Figures;
			var figures = new Vector2[settings.Count][];
			for ( var i = 0; i < settings.Count; i++ ) {
				figures[i] = settings[i].Elements;
			}
			return figures;
		}

		void Update() {
			_loop.Update(Time.deltaTime);
			_fieldPresenter.Draw(_state.Field);
			_figurePresenter.Draw(_state.Figure);
		}
	}
}