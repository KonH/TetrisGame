using UnityEngine;
using UnityEngine.Assertions;

namespace TetrisGame {
	public sealed class GameplayManager : MonoBehaviour {
		[SerializeField]
		FigureSpawnManager _figureSpawner;

		[SerializeField]
		ElementSpawnManager _elementSpawner;

		[SerializeField]
		ActiveFigureManager _figureManager;

		void OnValidate() {
			Assert.IsNotNull(_figureSpawner, nameof(_figureSpawner));
			Assert.IsNotNull(_elementSpawner, nameof(_elementSpawner));
			Assert.IsNotNull(_figureManager, nameof(_figureManager));
		}

		void Start() {
			CreateNewFigure();
		}

		void CreateNewFigure() {
			var figure = _figureSpawner.Spawn();
			_figureManager.ChangeActiveFigure(figure);
		}

		public void FinishFigure() {
			var active = _figureManager.Active;
			foreach ( var element in active.Elements ) {
				_elementSpawner.SpawnElement(element);
			}
			_figureSpawner.Recycle();
			CreateNewFigure();
		}
	}
}