using System.Collections.Generic;
using TetrisGame.Entity;
using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame.Settings {
	[CreateAssetMenu]
	public sealed class SessionSettings : ScriptableObject {
		[SerializeField]
		FigureEntity[] _figures;

		public IReadOnlyCollection<FigureEntity> Figures => _figures;

		void OnValidate() {
			Assert.AreNotEqual(0, _figures.Length, nameof(_figures));
		}
	}
}