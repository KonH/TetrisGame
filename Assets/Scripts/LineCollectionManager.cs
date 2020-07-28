using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame {
	public sealed class LineCollectionManager : MonoBehaviour {
		[SerializeField]
		FieldManager _field;

		[SerializeField]
		ElementManager _elementManager;

		[SerializeField]
		ElementSpawnManager _spawnManager;

		Dictionary<int, List<GameObject>> _lines    = new Dictionary<int, List<GameObject>>();
		Queue<List<GameObject>>           _linePool = new Queue<List<GameObject>>();

		void OnValidate() {
			Assert.IsNotNull(_field, nameof(_field));
			Assert.IsNotNull(_elementManager, nameof(_elementManager));
			Assert.IsNotNull(_spawnManager, nameof(_spawnManager));
		}

		public int TryCollapseLines() {
			var lines     = GroupByLines();
			var lineCount = TryCollapseFullLines(lines);
			if ( lineCount > 0 ) {
				_elementManager.RebuildPositions();
			}
			ClearCache();
			return lineCount;
		}

		Dictionary<int, List<GameObject>> GroupByLines() {
			var lines = _lines;
			foreach ( var element in _elementManager.Elements ) {
				var lineNumber = (int) element.transform.position.y;
				if ( !lines.TryGetValue(lineNumber, out var container) ) {
					container         = GetOrCreateContainer();
					lines[lineNumber] = container;
				}
				container.Add(element);
			}
			return lines;
		}

		List<GameObject> GetOrCreateContainer() {
			if ( _linePool.Count > 0 ) {
				return _linePool.Dequeue();
			}
			return new List<GameObject>();
		}

		int TryCollapseFullLines(Dictionary<int, List<GameObject>> lines) {
			var lineCount = 0;
			foreach ( var pair in lines ) {
				var items = pair.Value;
				if ( items.Count < _field.FieldWidth ) {
					continue;
				}
				foreach ( var element in items ) {
					_elementManager.RemoveElement(element);
					_spawnManager.Recycle(element);
				}
				lineCount++;
				DropHigherLines(lines, pair.Key);
			}
			return lineCount;
		}

		void DropHigherLines(Dictionary<int, List<GameObject>> lines, int aboveNumber) {
			foreach ( var pair in lines ) {
				if ( pair.Key <= aboveNumber ) {
					continue;
				}
				var items = pair.Value;
				foreach ( var element in items ) {
					element.transform.position += Vector3.down;
				}
			}
		}

		void ClearCache() {
			foreach ( var pair in _lines ) {
				var items = pair.Value;
				items.Clear();
				_linePool.Enqueue(items);
			}
			_lines.Clear();
		}
	}
}