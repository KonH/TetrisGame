using UnityEngine;

namespace TetrisGame {
	public sealed class Figure : MonoBehaviour {
		[Tooltip("Rotation is not suitable for some figures like boxes")]
		[SerializeField]
		public bool Fixed;

		public void Rotate(float angle) {
			if ( Fixed ) {
				return;
			}
			transform.Rotate(Vector3.forward, angle);
		}
	}
}