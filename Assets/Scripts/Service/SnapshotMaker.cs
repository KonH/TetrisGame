using System;
using System.Collections.Generic;
using System.IO;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	public sealed class SnapshotMaker {
		readonly GameState _state;
		readonly bool      _isEnabled;

		readonly SnapshotQueue _queue = new SnapshotQueue();

		public SnapshotMaker(GameState state) {
			_state     = state;
			_isEnabled = PlayerPrefs.GetInt("SaveSnapshot", 0) == 1;
		}

		public void TryTakeSnapshot() {
			if ( !ShouldTakeSnapshot() ) {
				return;
			}
			var snapshot = CreateSnapshot();
			_queue.Queue.Add(snapshot);
		}

		public void Save() {
			if ( !_isEnabled ) {
				return;
			}
			var json      = JsonUtility.ToJson(_queue);
			var targetDir = "Snapshots";
			var filePath  = $"{targetDir}/{Guid.NewGuid().ToString()}.json";
			if ( !Directory.Exists(targetDir) ) {
				Directory.CreateDirectory(targetDir);
			}
			File.WriteAllText(filePath, json);
		}

		bool ShouldTakeSnapshot() {
			return _isEnabled && (_state.Input != InputState.None);
		}

		SnapshotEntry CreateSnapshot() {
			var figure = new List<Vector2>(_state.Figure.Elements.Count);
			foreach ( var element in _state.Figure.Elements ) {
				figure.Add(element);
			}
			var field = new List<Vector2Int>();
			for ( var x = 0; x < _state.Field.Width; x++ ) {
				for ( var y = 0; y < _state.Field.Height; y++ ) {
					if ( _state.Field.GetState(x, y) ) {
						field.Add(new Vector2Int(x, y));
					}
				}
			}
			return new SnapshotEntry {
				InputState = _state.Input,
				FitCount = _state.FitCount,
				Origin = _state.Figure.Origin,
				Figure = figure,
				Field = field
			};
		}
	}
}