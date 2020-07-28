using UnityEngine;

namespace TetrisGame {
	public sealed class Figure : MonoBehaviour {
		[Tooltip("Rotation is not suitable for some figures like boxes")]
		[SerializeField]
		bool _fixedRotation;

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