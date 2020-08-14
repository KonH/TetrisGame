using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame.Settings {
	[Serializable]
	public sealed class GameSceneSettings {
		[SerializeField]
		Transform _elementRoot;

		[SerializeField]
		Transform _figureRoot;

		[SerializeField]
		TMP_Text _scoresText;

		public Transform ElementRoot => _elementRoot;
		public Transform FigureRoot  => _figureRoot;
		public TMP_Text  ScoresText  => _scoresText;

		public void Validate() {
			Assert.IsNotNull(_elementRoot, nameof(_elementRoot));
			Assert.IsNotNull(_figureRoot, nameof(_figureRoot));
			Assert.IsNotNull(_scoresText, nameof(_scoresText));
		}
	}
}