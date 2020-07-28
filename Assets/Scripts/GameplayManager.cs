using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame {
	public sealed class GameplayManager : MonoBehaviour {
		[SerializeField]
		FigureSpawnManager _spawner;

		[SerializeField]
		ActiveFigureManager _figureManager;

		void OnValidate() {
			Assert.IsNotNull(_spawner, nameof(_spawner));
			Assert.IsNotNull(_figureManager, nameof(_figureManager));
		}

		void Start() {
			CreateNewFigure();
		}

		void CreateNewFigure() {
			var figure = _spawner.Spawn();
			_figureManager.ChangeCurrentFigure(figure);
		}
	}
}