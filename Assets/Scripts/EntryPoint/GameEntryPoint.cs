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

		FieldState     _field;
		FieldPresenter _fieldPresenter;

		void OnValidate() {
			Assert.IsNotNull(_globalSettings, nameof(_globalSettings));
			_sceneSettings.Validate();
		}

		void Awake() {
			_field = new FieldState(_globalSettings.Width, _globalSettings.Height);
			var pool = new ElementPool(_sceneSettings.ElementRoot, _globalSettings.ElementPrefab);
			_fieldPresenter = new FieldPresenter(pool, _globalSettings.Width, _globalSettings.Height);
		}

		void Update() {
			_fieldPresenter.Draw(_field);
		}
	}
}