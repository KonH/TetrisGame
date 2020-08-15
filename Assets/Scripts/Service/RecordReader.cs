using System.Linq;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	public sealed class RecordReader {
		public void Read(RecordState state) {
			var data = PlayerPrefs.GetString(nameof(RecordState), string.Empty);
			var records = data
				.Split(',')
				.Select(str => int.TryParse(str, out var value) ? value : -1)
				.Where(v => (v > 0));
			state.Records.Clear();
			state.Records.AddRange(records);
		}
	}
}