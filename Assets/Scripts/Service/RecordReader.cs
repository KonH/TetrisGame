using System.Collections.Generic;
using System.Linq;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	/// <summary>
	/// Read records from PlayerPrefs
	/// </summary>
	public sealed class RecordReader {
		public void Read(RecordState state) {
			var data = PlayerPrefs.GetString(nameof(RecordState), string.Empty);
			state.Records.Clear();
			var inputs = data.Split(',');
			foreach ( var input in inputs ) {
				if ( RecordUnit.TryParse(input, out var unit) ) {
					state.Records.Add(unit);
				}
			}
		}
	}
}