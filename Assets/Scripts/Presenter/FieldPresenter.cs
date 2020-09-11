using System.Collections.Generic;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Presenter {
	/// <summary>
	/// Show field elements using element pool, update happens only when field is dirty (any change performed in loop),
	/// before returning elements to pool, try to re-use it to prevent unwanted activity changes
	/// </summary>
	public sealed class FieldPresenter {
		readonly ElementPool _pool;
		readonly Transform   _root;

		GameObject[,] _presentedState;

		Queue<GameObject> _elementsToRecycle = new Queue<GameObject>();
		Queue<Vector2Int> _positionsToPlace  = new Queue<Vector2Int>();


		public FieldPresenter(ElementPool pool, Transform root, int width, int height) {
			_pool           = pool;
			_root           = root;
			_presentedState = new GameObject[width, height];
		}

		public void Draw(IReadOnlyFieldState field) {
			if ( !field.IsDirty ) {
				return;
			}
			PrepareData(field);
			PresentNewElements();
			RecycleOldElements();
		}

		void PrepareData(IReadOnlyFieldState field) {
			_elementsToRecycle.Clear();
			_positionsToPlace.Clear();
			for ( var x = 0; x < field.Width; x++ ) {
				for ( var y = 0; y < field.Height; y++ ) {
					var isPresented       = _presentedState[x, y];
					var shouldBePresented = field.GetStateUnsafe(x, y);
					if ( isPresented == shouldBePresented ) {
						continue;
					}
					if ( shouldBePresented ) {
						_positionsToPlace.Enqueue(new Vector2Int(x, y));
					} else {
						_elementsToRecycle.Enqueue(_presentedState[x, y]);
						_presentedState[x, y] = null;
					}
				}
			}
		}

		void PresentNewElements() {
			while ( _positionsToPlace.Count > 0 ) {
				var position = _positionsToPlace.Dequeue();
				GameObject element;
				if ( _elementsToRecycle.Count > 0 ) {
					element = _elementsToRecycle.Dequeue();
					element.transform.localPosition = new Vector2(position.x, position.y);
				} else {
					element = _pool.GetOrCreate(_root, position);
				}
				_presentedState[position.x, position.y] = element;
			}
		}

		void RecycleOldElements() {
			while ( _elementsToRecycle.Count > 0 ) {
				_pool.Recycle(_elementsToRecycle.Dequeue());
			}
		}
	}
}