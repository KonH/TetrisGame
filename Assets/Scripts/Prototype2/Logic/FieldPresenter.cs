using UnityEngine;

namespace TetrisGame.Logic {
	public sealed class FieldPresenter {
		readonly ElementPool   _pool;
		readonly GameObject[,] _state;

		public FieldPresenter(int width, int height, ElementPool pool) {
			_pool  = pool;
			_state = new GameObject[width, height];
		}

		public void Present(bool[,] field) {
			var width  = _state.GetLength(0);
			var height = _state.GetLength(0);
			for ( var x = 0; x < width; x++ ) {
				for ( var y = 0; y < height; y++ ) {
					var actualState = field[x, y];
					var presentState = _state[x, y];
					var isDirty     = ((bool)presentState != actualState);
					if ( !isDirty ) {
						continue;
					}
					if ( actualState ) {
						_state[x, y] = _pool.GetOrCreate(new Vector3(x, y));
					} else {
						_pool.Recycle(presentState);
						_state[x, y] = null;
					}
				}
			}
		}
	}
}