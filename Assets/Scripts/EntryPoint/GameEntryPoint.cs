using TetrisGame.Presenter;
using TetrisGame.Service;
using TetrisGame.Settings;
using TetrisGame.State;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace TetrisGame.EntryPoint {
	/// <summary>
	/// Start game simulation, send input state and reflect state changes
	/// </summary>
	public sealed class GameEntryPoint : MonoBehaviour {
		[SerializeField]
		GameGlobalSettings _globalSettings;

		[SerializeField]
		GameGeneticSettings _geneticSettings;

		[SerializeField]
		GameSceneSettings _sceneSettings;

		IGameLoop       _loop;
		FieldPresenter  _fieldPresenter;
		FigurePresenter _figurePresenter;
		ScorePresenter  _scorePresenter;
		SpeedPresenter  _speedPresenter;
		FitPresenter    _fitPresenter;

		bool _finished;

		void OnValidate() {
			Assert.IsNotNull(_globalSettings, nameof(_globalSettings));
			Assert.IsNotNull(_geneticSettings, nameof(_geneticSettings));
			_sceneSettings.Validate();
		}

		void Awake() {
			var settings = GameLoopSettingsFactory.Create(_globalSettings, Random.Range(0, int.MaxValue));
			_loop = CreateGameLoop(settings, _geneticSettings.Settings);
			var pool = new ElementPool(_globalSettings.ElementPrefab);
			_fieldPresenter = new FieldPresenter(
				pool, _sceneSettings.ElementRoot, _globalSettings.Width, _globalSettings.Height);
			_figurePresenter = new FigurePresenter(pool, _sceneSettings.FigureRoot);
			_scorePresenter = new ScorePresenter(_sceneSettings.ScoresText, _sceneSettings.RewardAudioSource);
			_speedPresenter = new SpeedPresenter(_sceneSettings.SpeedText);
			_fitPresenter = new FitPresenter(_sceneSettings.FitAudioSource);

			_sceneSettings.RecordListView.Hide();
		}

		IGameLoop CreateGameLoop(GameLoopSettings loopSettings, GeneticSettings geneticSettings) {
			var useGenetic = GameRuntimeSettings.Instance.UseAI;
			if ( useGenetic ) {
				return new GeneticGameLoop(loopSettings, geneticSettings, true);
			}
			return new PlayerGameLoop(loopSettings);
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
			_fitPresenter.Draw(state.FitCount);
			if ( state.Finished ) {
				_finished = true;
				_sceneSettings.RecordListView.Show(state, HandleRestart, HandleMenu);
				_sceneSettings.GameOverAudioSource.Play();
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

		void HandleRestart() {
			SceneManager.LoadScene("Game");
		}

		void HandleMenu() {
			SceneManager.LoadScene("Menu");
		}
	}
}