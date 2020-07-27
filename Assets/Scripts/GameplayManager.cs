using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame {
	public sealed class GameplayManager : MonoBehaviour {
		[SerializeField]
		FigureSpawnManager FigureSpawner;

		[SerializeField]
		FigureManager FigureManager;

		void OnValidate() {
			Assert.IsNotNull(FigureSpawner, nameof(FigureSpawner));
			Assert.IsNotNull(FigureManager, nameof(FigureManager));
		}

		void Start() {
			CreateNewFigure();
		}

		void CreateNewFigure() {
			FigureManager.CurrentFigure = FigureSpawner.Spawn();
		}
	}
}