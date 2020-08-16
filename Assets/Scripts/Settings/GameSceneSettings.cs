using System;
using TetrisGame.Presenter;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame.Settings {
	[Serializable]
	public sealed class GameSceneSettings {
		[SerializeField]
		Transform _elementRoot;

		[SerializeField]
		Transform _figureRoot;

		[SerializeField]
		TMP_Text _scoresText;

		[SerializeField]
		TMP_Text _speedText;

		[SerializeField]
		RecordListView _recordListView;

		[SerializeField]
		AudioSource _fitAudioSource;

		[SerializeField]
		AudioSource _rewardAudioSource;

		[SerializeField]
		AudioSource _gameOverAudioSource;

		public Transform ElementRoot => _elementRoot;
		public Transform FigureRoot  => _figureRoot;
		public TMP_Text  ScoresText  => _scoresText;
		public TMP_Text  SpeedText   => _speedText;

		public RecordListView RecordListView => _recordListView;

		public AudioSource FitAudioSource      => _fitAudioSource;
		public AudioSource RewardAudioSource   => _rewardAudioSource;
		public AudioSource GameOverAudioSource => _gameOverAudioSource;

		public void Validate() {
			Assert.IsNotNull(_elementRoot, nameof(_elementRoot));
			Assert.IsNotNull(_figureRoot, nameof(_figureRoot));
			Assert.IsNotNull(_scoresText, nameof(_scoresText));
			Assert.IsNotNull(_speedText, nameof(_speedText));
			Assert.IsNotNull(_recordListView, nameof(_recordListView));
			Assert.IsNotNull(_fitAudioSource, nameof(_fitAudioSource));
			Assert.IsNotNull(_rewardAudioSource, nameof(_rewardAudioSource));
			Assert.IsNotNull(_gameOverAudioSource, nameof(_gameOverAudioSource));
		}
	}
}