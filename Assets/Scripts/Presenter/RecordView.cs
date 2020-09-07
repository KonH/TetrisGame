using TetrisGame.State;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame.Presenter {
	public sealed class RecordView : MonoBehaviour {
		static readonly Color CurrentColor = Color.green;

		[SerializeField]
		TMP_Text _text;

		public void OnValidate() {
			Assert.IsNotNull(_text, nameof(_text));
		}

		public void Draw(bool found, bool current, RecordUnit unit) {
			_text.text = found ? unit.ToString() : "-";
			if ( current ) {
				_text.color = CurrentColor;
			}
		}
	}
}