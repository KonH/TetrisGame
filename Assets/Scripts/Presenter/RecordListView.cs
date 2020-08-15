using TetrisGame.State;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame.Presenter {
	public sealed class RecordListView : MonoBehaviour {
		[SerializeField]
		RecordView[] _records;

		public void OnValidate() {
			Assert.AreNotEqual(0, _records?.Length, nameof(_records));
		}

		public void Show(IReadOnlyGameState state) {
			var records = state.Records.Records;
			for ( var i = 0; i < _records.Length; i++ ) {
				var record = (i < records.Count) ? records[i] : -1;
				_records[i].Draw(record > 0, state.Scores == record, record);
			}
			gameObject.SetActive(true);
		}

		public void Hide() {
			gameObject.SetActive(false);
		}
	}
}