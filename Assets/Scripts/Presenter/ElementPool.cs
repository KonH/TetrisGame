using System.Collections.Generic;
using UnityEngine;

namespace TetrisGame.Presenter {
    public sealed class ElementPool {
        readonly Transform  _parent;
        readonly GameObject _prefab;

        readonly Queue<GameObject> _pooledElements = new Queue<GameObject>();

        public ElementPool(Transform parent, GameObject prefab) {
            _parent = parent;
            _prefab = prefab;
        }

        public GameObject GetOrCreate(Vector2 position) {
            if ( _pooledElements.Count <= 0 ) {
                return GameObject.Instantiate(_prefab, position, Quaternion.identity, _parent);
            }
            var element = _pooledElements.Dequeue();
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
