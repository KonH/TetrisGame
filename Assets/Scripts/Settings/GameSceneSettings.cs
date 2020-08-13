using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame.Settings {
	[Serializable]
	public sealed class GameSceneSettings {
		[SerializeField]
		Transform _elementRoot;

		[SerializeField]
		Transform _figureRoot;

		public Transform ElementRoot => _elementRoot;
		public Transform FigureRoot  => _figureRoot;

		public void Validate() {
			Assert.IsNotNull(_elementRoot, nameof(_elementRoot));
			Assert.IsNotNull(_figureRoot, nameof(_figureRoot));
		}
	}
}