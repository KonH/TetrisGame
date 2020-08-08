using TetrisGame.Entity;
using UnityEngine;

namespace TetrisGame.Logic {
	public sealed class FigurePool {
		readonly FigureEntity[] _pool;

		public FigurePool(int count) {
			_pool = new FigureEntity[count];
		}

		public bool TryGet(int index, out FigureEntity figure) {
			figure = _pool[index];
			if ( !figure ) {
				return false;
			}
			figure.gameObject.SetActive(true);
			var trans = figure.transform;
			trans.localPosition = Vector3.zero;
			trans.rotation = Quaternion.identity;
			return true;
		}

		public void Recycle(int index, FigureEntity figure) {
			figure.gameObject.SetActive(false);
			_pool[index] = figure;
		}
	}
}