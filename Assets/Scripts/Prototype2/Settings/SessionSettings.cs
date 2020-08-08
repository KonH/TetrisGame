using System.Collections.Generic;
using TetrisGame.Entity;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame.Settings {
	[CreateAssetMenu]
	public sealed class SessionSettings : ScriptableObject {
		[SerializeField]
		int _width;

		[SerializeField]
		int _height;

		[SerializeField]
		FigureEntity[] _figures;

		[SerializeField]
		GameObject _element;

		[SerializeField]
		float _initialSpeed;

		public int Width  => _width;
		public int Height => _height;

		public IReadOnlyList<FigureEntity> Figures => _figures;

		public GameObject Element => _element;

		public float InitialSpeed => _initialSpeed;

		void OnValidate() {
			Assert.AreNotEqual(0, _figures.Length, nameof(_figures));
			Assert.AreNotEqual(0, _width, nameof(_width));
			Assert.AreNotEqual(0, _height, nameof(_height));
			Assert.AreNotEqual(0, _initialSpeed, nameof(_initialSpeed));
		}
	}
}