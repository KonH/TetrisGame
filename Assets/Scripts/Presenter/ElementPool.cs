using System.Collections.Generic;
using UnityEngine;

namespace TetrisGame.Presenter {
    /// <summary>
    /// Service class to re-use element objects
    /// </summary>
    public sealed class ElementPool {
        readonly GameObject _prefab;

        readonly Queue<GameObject> _pooledElements = new Queue<GameObject>();

        public ElementPool(GameObject prefab) {
            _prefab = prefab;
        }

        public GameObject GetOrCreate(Transform parent, Vector2 position) {
            if ( _pooledElements.Count <= 0 ) {
                return GameObject.Instantiate(_prefab, position, Quaternion.identity, parent);
            }
            var element = _pooledElements.Dequeue();
            element.transform.parent = parent;
            element.transform.localPosition = position;
            element.SetActive(true);
            return element;
        }

        public void Recycle(GameObject element) {
            element.SetActive(false);
            _pooledElements.Enqueue(element);
        }
    }
}
