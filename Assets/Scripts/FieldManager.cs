using UnityEngine;

namespace TetrisGame {
	public sealed class FieldManager : MonoBehaviour {
		[SerializeField]
		int _fieldWidth;

		public float FieldWidth => _fieldWidth;
	}
}