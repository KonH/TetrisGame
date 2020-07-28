using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame {
	public sealed class Figure : MonoBehaviour {
		[Tooltip("Rotation is not suitable for some figures like boxes")]
		[SerializeField]
		bool _fixedRotation;

		[SerializeField]
		Transform[] _elements;

		public IReadOnlyCollection<Transform> Elements => _elements;

		void OnValidate() {
			Assert.AreNotEqual(0, _elements.Length, nameof(_elements));
		}

		[ContextMenu("Fill")]
		void Fill() {
			_elements = new Transform[transform.childCount];
			for ( var i = 0; i < transform.childCount; i++ ) {
				_elements[i] = transform.GetChild(i);
			}
		}

		public void Rotate(float angle) {
			if ( _fixedRotation ) {
				return;
			}
			transform.Rotate(Vector3.forward, angle);
		}

		public void Move(Vector3 direction) {
			transform.position += direction;
		}
	}
}