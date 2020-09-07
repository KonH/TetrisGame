using System.Collections.Generic;
using System.Linq;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	/// <summary>
	/// Write unique records to PlayerPrefs
	/// </summary>
	public sealed class RecordWriter {
		public void Write(RecordState state, RecordUnit unit) {
			var records = new List<RecordUnit>(state.Records) { unit }
				.Distinct()
				.OrderByDescending(u => u.Scores);
			state.Records.Clear();
			state.Records.AddRange(records);
			Flush(state);
		}

		void Flush(RecordState state) {
			var data = string.Join(",", state.Records.Select(v => v.ToString()));
			PlayerPrefs.SetString(nameof(RecordState), data);
			PlayerPrefs.Save();
		}
	}
}