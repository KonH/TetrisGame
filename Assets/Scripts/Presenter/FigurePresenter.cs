using System.Collections.Generic;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Presenter {
	/// <summary>
	/// Show figure elements using element pool, sync happens each frame because of constant falling
	/// </summary>
	public sealed class FigurePresenter {
		readonly ElementPool _pool;
		readonly Transform   _root;

		readonly List<GameObject> _elements = new List<GameObject>();

		public FigurePresenter(ElementPool pool, Transform root) {
			_pool = pool;
			_root = root;
		}

		public void Draw(IReadOnlyFigureState figure) {
			_root.transform.localPosition = figure.Origin;
			SyncElementCount(figure.Elements);
			SyncElements(figure.Elements);
		}

		void SyncElementCount(IReadOnlyList<Vector2Int> elements) {
			RemoveExcessiveElements(elements);
			AddRequiredElements(elements);
		}

		void RemoveExcessiveElements(IReadOnlyList<Vector2Int> elements) {
			while ( _elements.Count > elements.Count ) {
				var lastElement = _elements[_elements.Count - 1];
				_pool.Recycle(lastElement);
				_elements.RemoveAt(_elements.Count - 1);
			}
		}

		void AddRequiredElements(IReadOnlyList<Vector2Int> elements) {
			while ( elements.Count > _elements.Count ) {
				_elements.Add(_pool.GetOrCreate(_root, Vector2Int.zero));
			}
		}

		void SyncElements(IReadOnlyList<Vector2Int> elements) {
			for ( var i = 0; i < elements.Count; i++ ) {
				_elements[i].transform.localPosition = (Vector2)elements[i];
			}
		}
	}
}