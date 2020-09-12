using TetrisGame.Settings;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TetrisGame.EntryPoint {
	public sealed class MenuEntryPoint : MonoBehaviour {
		[SerializeField]
		Button _playButton;

		[SerializeField]
		Button _aiButton;

		void OnValidate() {
			Assert.IsNotNull(_playButton, nameof(_playButton));
			Assert.IsNotNull(_aiButton, nameof(_aiButton));
		}

		void Awake() {
			_playButton.onClick.AddListener(OnPlay);
			_aiButton.onClick.AddListener(OnAI);
		}

		void Start() {
			QualitySettings.vSyncCount  = 0;
			Application.targetFrameRate = 60;
		}

		void OnPlay() {
			GameRuntimeSettings.Instance.UseAI = false;
			SceneManager.LoadScene("Game");
		}

		void OnAI() {
			GameRuntimeSettings.Instance.UseAI = true;
			SceneManager.LoadScene("Game");
		}
	}
}