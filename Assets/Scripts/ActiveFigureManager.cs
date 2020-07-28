using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame {
	public sealed class ActiveFigureManager : MonoBehaviour {
		[SerializeField]
		FieldManager _field;

		public Figure Active { get; private set; }

		void OnValidate() {
			Assert.IsNotNull(_field, nameof(_field));
		}

		public void ChangeActiveFigure(Figure figure) {
			Assert.IsNotNull(figure);
			Active = figure;
		}

		public void TryRotate(float angle) {
			if ( !Active ) {
				return;
			}
			Active.Rotate(angle);
			// After rotation figure can not be fit in borders in some cases
			// So it's required to move it inside valid place
			while ( !_field.IsInsideBorders(Active) ) {
				var offset = -Mathf.Sign(Active.transform.position.x);
				Active.Move(offset * Vector3.right);
			}
		}

		public void TryMove(Vector3 direction) {
			if ( !Active ) {
				return;
			}
			if ( !_field.IsInsideBorders(Active, direction) ) {
				return;
			}
			Active.Move(direction);
		}
	}
}