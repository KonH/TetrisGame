using UnityEngine;

namespace TetrisGame {
	public sealed class FieldManager : MonoBehaviour {
		[SerializeField]
		int _fieldWidth;

		[SerializeField]
		int _fieldHeight;

		public int LeftBorder  { get; private set; }
		public int RightBorder { get; private set; }

		public int FieldWidth => _fieldWidth;

		int _bottom;

		void Awake() {
			AssignBorders();
		}

		void AssignBorders() {
			var halfWidth = (_fieldWidth / 2);
			var position = transform.position;
			var x = (int) position.x;
			LeftBorder  = x - halfWidth + 1; // Field is even, one border is closer
			RightBorder = x + halfWidth;
			_bottom = (int) position.y - (_fieldHeight / 2);
		}

		public bool IsInsideBorders(Figure figure, Vector3 offset = default) {
			foreach ( var element in figure.Elements ) {
				var position = element.position + offset;
				var x        = position.x;
				if ( (x < LeftBorder) || (x > RightBorder) ) {
					return false;
				}
			}
			return true;
		}

		public bool IsOnBottom(Figure figure) {
			foreach ( var element in figure.Elements ) {
				var y = element.position.y;
				if ( y <= _bottom ) {
					return true;
				}
			}
			return false;
		}
	}
}