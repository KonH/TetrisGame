using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame.Settings {
	public sealed class FigureSettings : MonoBehaviour {
		[SerializeField]
		Vector2[] _elements;

		public Vector2Int[] Elements =>
			_elements
				.Select(v => new Vector2Int((int)v.x, (int)v.y))
				.ToArray();

		public void OnValidate() {
			Assert.AreNotEqual(0, _elements?.Length, nameof(_elements));
		}

		[ContextMenu(nameof(Fill))]
		public void Fill() {
			_elements = new Vector2[transform.childCount];
			for ( var i = 0; i < transform.childCount; i++ ) {
				_elements[i] = transform.GetChild(i).position;
			}
		}
	}
}