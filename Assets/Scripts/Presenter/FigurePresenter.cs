using System.Collections.Generic;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Presenter {
	public sealed class FigurePresenter {
		readonly ElementPool _pool;
		readonly Transform   _root;

		readonly List<GameObject> _elements = new List<GameObject>();

		public FigurePresenter(ElementPool pool, Transform root) {
			_pool = pool;
			_root = root;
		}

		public void Draw(FigureState figure) {
			_root.transform.localPosition = figure.Origin;
			SyncElementCount(figure.Elements);
			SyncElements(figure.Elements);
		}

		void SyncElementCount(IReadOnlyList<Vector2> elements) {
			RemoveExcessiveElements(elements);
			AddRequiredElements(elements);
		}

		void RemoveExcessiveElements(IReadOnlyList<Vector2> elements) {
			while ( _elements.Count > elements.Count ) {
				var lastElement = _elements[_elements.Count - 1];
				_pool.Recycle(lastElement);
				_elements.RemoveAt(_elements.Count - 1);
			}
		}

		void AddRequiredElements(IReadOnlyList<Vector2> elements) {
			while ( elements.Count > _elements.Count ) {
				_elements.Add(_pool.GetOrCreate(_root, Vector2.zero));
			}
		}

		void SyncElements(IReadOnlyList<Vector2> elements) {
			for ( var i = 0; i < elements.Count; i++ ) {
				_elements[i].transform.localPosition = elements[i];
			}
		}
	}
}