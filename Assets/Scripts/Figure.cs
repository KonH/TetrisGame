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

		public Vector3 ScheduledDirection { get; private set; }

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

		public void ScheduleMove(Vector3 direction) {
			ScheduledDirection += direction;
		}

		public void ApplyMove() {
			PerformMove(ScheduledDirection);
			ScheduledDirection = Vector3.zero;
		}

		public void PerformMove(Vector3 direction) {
			transform.position += direction;
		}

		public void Show(Vector3 position) {
			var trans = transform;
			trans.position = position;
			trans.rotation = Quaternion.identity;
			gameObject.SetActive(true);
		}

		public void Hide() {
			ScheduledDirection = Vector3.zero;
			gameObject.SetActive(false);
		}
	}
}