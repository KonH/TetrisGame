using System.Collections.Generic;
using UnityEngine;

namespace TetrisGame {
	public sealed class ElementManager : MonoBehaviour {
		List<GameObject> _elements  = new List<GameObject>();
		List<Vector3>    _positions = new List<Vector3>();

		public IReadOnlyCollection<GameObject> Elements  => _elements;
		public IReadOnlyCollection<Vector3>    Positions => _positions;

		public void AddElement(GameObject go) {
			_elements.Add(go);
		}

		public void RemoveElement(GameObject go) {
			_elements.Remove(go);
		}

		public void RebuildPositions() {
			_positions.Clear();
			foreach ( var element in _elements ) {
				_positions.Add(element.transform.position);
			}
		}
	}
}