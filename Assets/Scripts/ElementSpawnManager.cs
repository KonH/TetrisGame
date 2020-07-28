using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame {
	public sealed class ElementSpawnManager : MonoBehaviour {
		[SerializeField]
		GameObject _prefab;

		[Tooltip("Parent for new elements")]
		[SerializeField]
		Transform _spawnContainer;

		Queue<GameObject> _pooledElements = new Queue<GameObject>();

		void OnValidate() {
			Assert.IsNotNull(_prefab, nameof(_prefab));
			Assert.IsNotNull(_spawnContainer, nameof(_spawnContainer));
		}

		public GameObject SpawnElement(Transform origin) {
			if ( _pooledElements.Count <= 0 ) {
				return Instantiate(_prefab, origin.position, Quaternion.identity, _spawnContainer);
			}
			var pooledElement = _pooledElements.Dequeue();
			pooledElement.transform.position = origin.position;
			pooledElement.SetActive(true);
			return pooledElement;
		}

		public void Recycle(GameObject element) {
			element.SetActive(false);
			_pooledElements.Enqueue(element);
		}
	}
}