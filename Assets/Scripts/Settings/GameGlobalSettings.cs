using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame.Settings {
	[CreateAssetMenu]
	public sealed class GameGlobalSettings : ScriptableObject {
		[SerializeField]
		int _width;

		[SerializeField]
		int _height;

		[SerializeField]
		GameObject _elementPrefab;

		[SerializeField]
		FigureSettings[] _figures;

		public int Width  => _width;
		public int Height => _height;

		public GameObject ElementPrefab => _elementPrefab;

		public IReadOnlyList<FigureSettings> Figures => _figures;

		public void OnValidate() {
			Assert.AreNotEqual(0, _width, nameof(_width));
			Assert.AreNotEqual(0, _height, nameof(_height));
			Assert.IsNotNull(_elementPrefab, nameof(_elementPrefab));
			Assert.AreNotEqual(0, _figures?.Length, nameof(_figures));
		}
	}
}