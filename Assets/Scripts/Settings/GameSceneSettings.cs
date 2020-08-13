using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame.Settings {
	[Serializable]
	public sealed class GameSceneSettings {
		[SerializeField]
		Transform _elementRoot;

		public Transform ElementRoot => _elementRoot;

		public void Validate() {
			Assert.IsNotNull(_elementRoot, nameof(_elementRoot));
		}
	}
}