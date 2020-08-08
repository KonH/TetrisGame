using System.Collections.Generic;
using UnityEngine;

namespace TetrisGame.Logic {
	public sealed class ElementPool {
		readonly GameObject _prefab;
		readonly Transform  _parent;

		readonly Queue<GameObject> _queue = new Queue<GameObject>();

		public ElementPool(GameObject prefab, Transform parent) {
			_prefab = prefab;
			_parent = parent;
		}

		public GameObject GetOrCreate(Vector3 position) {
			if ( _queue.Count > 0 ) {
				var pooledElement = _queue.Dequeue();
				pooledElement.transform.position = position;
				pooledElement.gameObject.SetActive(true);
				return pooledElement;
			}
			return GameObject.Instantiate(_prefab, position, Quaternion.identity, _parent);
		}

		public void Recycle(GameObject element) {
			_queue.Enqueue(element);
			element.gameObject.SetActive(false);
		}
	}
}