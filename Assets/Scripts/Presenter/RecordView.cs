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

		public void Draw(bool found, bool current, int score = 0) {
			_text.text = found ? score.ToString() : "-";
			if ( current ) {
				_text.color = CurrentColor;
			}
		}
	}
}