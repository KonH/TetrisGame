using TetrisGame.Presenter;
using TetrisGame.Service;
using TetrisGame.Settings;
using TetrisGame.State;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace TetrisGame.EntryPoint {
	public sealed class GameEntryPoint : MonoBehaviour {
		[SerializeField]
		GameGlobalSettings _globalSettings;

		[SerializeField]
		GameSceneSettings _sceneSettings;

		GameLoop        _loop;
		FieldPresenter  _fieldPresenter;
		FigurePresenter _figurePresenter;
		ScorePresenter  _scorePresenter;
		SpeedPresenter  _speedPresenter;

		bool _finished;

		void OnValidate() {
			Assert.IsNotNull(_globalSettings, nameof(_globalSettings));
			_sceneSettings.Validate();
		}

		void Awake() {
			_loop = new GameLoop(
				_globalSettings.Width, _globalSettings.Height,
				_globalSettings.InitialSpeed, _globalSettings.LinesToIncrease, _globalSettings.IncreaseValue,
				PopulateFigures(), _globalSettings.ScorePerLines);
			var pool = new ElementPool(_globalSettings.ElementPrefab);
			_fieldPresenter = new FieldPresenter(
				pool, _sceneSettings.ElementRoot, _globalSettings.Width, _globalSettings.Height);
			_figurePresenter = new FigurePresenter(pool, _sceneSettings.FigureRoot);
			_scorePresenter = new ScorePresenter(_sceneSettings.ScoresText);
			_speedPresenter = new SpeedPresenter(_sceneSettings.SpeedText);

			_sceneSettings.RecordListView.Hide();
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
			if ( _finished ) {
				return;
			}
			var state = _loop.State;
			_loop.Update(Time.deltaTime);
			_fieldPresenter.Draw(state.Field);
			_figurePresenter.Draw(state.Figure);
			_scorePresenter.Draw(state.Scores);
			_speedPresenter.Draw(state.Speed.Level);
			if ( state.Finished ) {
				_finished = true;
				_sceneSettings.RecordListView.Show(state);
			}
		}

		public void HandleMoveLeft(InputAction.CallbackContext ctx) => HandleInput(ctx, InputState.MoveLeft);
		public void HandleMoveRight(InputAction.CallbackContext ctx) => HandleInput(ctx, InputState.MoveRight);
		public void HandleSpeedUp(InputAction.CallbackContext ctx) => HandleInput(ctx, InputState.SpeedUp);
		public void HandleRotate(InputAction.CallbackContext ctx) => HandleInput(ctx, InputState.Rotate);

		void HandleInput(InputAction.CallbackContext ctx, InputState input) {
			if ( ctx.started ) {
				_loop.UpdateInput(input);
			}
		}
	}
}