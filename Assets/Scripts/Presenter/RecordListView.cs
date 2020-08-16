using System;
using TetrisGame.State;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace TetrisGame.Presenter {
	public sealed class RecordListView : MonoBehaviour {
		[SerializeField]
		Animation _animation;

		[SerializeField]
		RecordView[] _records;

		[SerializeField]
		Button _restartButton;

		Action _restartCallback = () => {};

		public void OnValidate() {
			Assert.AreNotEqual(0, _records?.Length, nameof(_records));
			Assert.IsNotNull(_restartButton, nameof(_restartButton));
		}

		public void Awake() {
			_restartButton.onClick.AddListener(() => _restartCallback());
		}

		public void Show(IReadOnlyGameState state, Action restartCallback) {
			var records = state.Records.Records;
			for ( var i = 0; i < _records.Length; i++ ) {
				var record = (i < records.Count) ? records[i] : -1;
				_records[i].Draw(record > 0, state.Scores == record, record);
			}
			gameObject.SetActive(true);
			_restartCallback = restartCallback;
			if ( _animation ) {
				_animation.Play();
			}
		}

		public void Hide() {
			gameObject.SetActive(false);
		}
	}
}