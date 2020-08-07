using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame.Entity {
	public sealed class FigureEntity : MonoBehaviour {
		[Tooltip("Rotation is not suitable for some figures like boxes")]
		[SerializeField]
		bool _fixedRotation;

		[SerializeField]
		Transform[] _elements;

		public bool FixedRotation => _fixedRotation;

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
	}
}