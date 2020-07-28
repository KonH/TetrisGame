using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame {
	public sealed class ActiveFigureManager : MonoBehaviour {
		[SerializeField]
		FieldManager _field;

		Figure _current;

		void OnValidate() {
			Assert.IsNotNull(_field, nameof(_field));
		}

		public void ChangeCurrentFigure(Figure figure) {
			Assert.IsNotNull(figure);
			_current = figure;
		}

		public void TryRotate(float angle) {
			if ( !_current ) {
				return;
			}
			_current.Rotate(angle);
			// After rotation figure can not be fit in borders in some cases
			// So it's required to move it inside valid place
			while ( !_field.IsInsideBorders(_current) ) {
				var offset = -Mathf.Sign(_current.transform.position.x);
				_current.Move(offset * Vector3.right);
			}
		}

		public void TryMove(Vector3 direction) {
			if ( !_current ) {
				return;
			}
			if ( !_field.IsInsideBorders(_current, direction) ) {
				return;
			}
			_current.Move(direction);
		}
	}
}