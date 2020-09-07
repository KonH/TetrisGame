using System;
using TetrisGame.State;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace TetrisGame.Presenter {
	/// <summary>
	/// Represents all records and mark current one with color
	/// </summary>
	public sealed class RecordListView : MonoBehaviour {
		[SerializeField]
		Animation _animation;

		[SerializeField]
		RecordView[] _records;

		[SerializeField]
		Button _restartButton;

		[SerializeField]
		Button _menuButton;

		Action _restartCallback = () => {};
		Action _menuCallback = () => {};

		public void OnValidate() {
			Assert.AreNotEqual(0, _records?.Length, nameof(_records));
			Assert.IsNotNull(_restartButton, nameof(_restartButton));
			Assert.IsNotNull(_menuButton, nameof(_menuButton));
		}

		public void Awake() {
			_restartButton.onClick.AddListener(() => _restartCallback());
			_menuButton.onClick.AddListener(() => _menuCallback());
		}

		public void Show(IReadOnlyGameState state, Action restartCallback, Action menuCallback) {
			var records = state.Records.Records;
			for ( var i = 0; i < _records.Length; i++ ) {
				var record = (i < records.Count) ? records[i] : default;
				_records[i].Draw(record.Scores > 0, state.Scores == record.Scores, record);
			}
			gameObject.SetActive(true);
			_restartCallback = restartCallback;
			_menuCallback = menuCallback;
			if ( _animation ) {
				_animation.Play();
			}
		}

		public void Hide() {
			gameObject.SetActive(false);
		}
	}
}