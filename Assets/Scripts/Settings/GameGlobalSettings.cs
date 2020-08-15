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
		float _initialSpeed;

		[SerializeField]
		int _linesToIncrease;

		[SerializeField]
		float _increaseValue;

		[SerializeField]
		GameObject _elementPrefab;

		[SerializeField]
		FigureSettings[] _figures;

		[SerializeField]
		int[] _scorePerLines;

		public int Width  => _width;
		public int Height => _height;

		public float InitialSpeed    => _initialSpeed;
		public int   LinesToIncrease => _linesToIncrease;
		public float IncreaseValue   => _increaseValue;

		public GameObject ElementPrefab => _elementPrefab;

		public IReadOnlyList<FigureSettings> Figures => _figures;

		public IReadOnlyList<int> ScorePerLines => _scorePerLines;

		public void OnValidate() {
			Assert.AreNotEqual(0, _width, nameof(_width));
			Assert.AreNotEqual(0, _height, nameof(_height));
			Assert.AreNotEqual(0.0f, _initialSpeed, nameof(_initialSpeed));
			Assert.AreNotEqual(0, _linesToIncrease, nameof(_linesToIncrease));
			Assert.AreNotEqual(0.0f, _increaseValue, nameof(_increaseValue));
			Assert.IsNotNull(_elementPrefab, nameof(_elementPrefab));
			Assert.AreNotEqual(0, _figures?.Length, nameof(_figures));
			Assert.AreNotEqual(0, _scorePerLines?.Length, nameof(_scorePerLines));
		}
	}
}