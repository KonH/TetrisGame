using System.Collections.Generic;
using TetrisGame.Entity;
using UnityEngine;

namespace TetrisGame.Logic {
	public sealed class FigureSpawner {
		readonly FigurePool                  _pool;
		readonly IReadOnlyList<FigureEntity> _prefabs;
		readonly Transform                   _parent;

		public FigureSpawner(FigurePool pool, IReadOnlyList<FigureEntity> prefabs, Transform parent) {
			_pool    = pool;
			_prefabs = prefabs;
			_parent  = parent;
		}

		public FigureState Spawn() {
			var index = Random.Range(0, _prefabs.Count);
			if ( !_pool.TryGet(index, out var figure) ) {
				figure = GameObject.Instantiate(_prefabs[index], _parent, false);
			}
			return new FigureState {
				IsPresent = true,
				Entity    = figure,
				Index     = index
			};
		}
	}
}