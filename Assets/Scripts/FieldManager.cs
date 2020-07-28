using UnityEngine;

namespace TetrisGame {
	public sealed class FieldManager : MonoBehaviour {
		[SerializeField]
		int _fieldWidth;

		[SerializeField]
		int _fieldHeight;

		public int LeftBorder  { get; private set; }
		public int RightBorder { get; private set; }

		void Awake() {
			AssignBorders();
		}

		void AssignBorders() {
			var halfWidth = (_fieldWidth / 2);
			var x = (int) transform.position.x;
			LeftBorder  = x - halfWidth;
			RightBorder = x + halfWidth;
		}

		public bool IsInsideBorders(Figure figure, Vector3 offset = default) {
			foreach ( var element in figure.Elements ) {
				var position = element.position + offset;
				var x = position.x;
				if ( (x < LeftBorder) || (x > RightBorder) ) {
					return false;
				}
			}
			return true;
		}
	}
}