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

		[SerializeField]
		ElementManager _elementManager;

		void OnValidate() {
			Assert.IsNotNull(_figureSpawner, nameof(_figureSpawner));
			Assert.IsNotNull(_elementSpawner, nameof(_elementSpawner));
			Assert.IsNotNull(_figureManager, nameof(_figureManager));
			Assert.IsNotNull(_elementManager, nameof(_elementManager));
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
			foreach ( var trans in active.Elements ) {
				var position = Align(trans.position);
				var element = _elementSpawner.SpawnElement(position);
				_elementManager.AddElement(element);
			}
			_elementManager.RebuildPositions();
			_figureSpawner.Recycle();
			CreateNewFigure();
		}

		Vector3 Align(Vector3 inaccuratePosition) {
			return new Vector3(
				Mathf.Round(inaccuratePosition.x),
				Mathf.Round(inaccuratePosition.y),
				Mathf.Round(inaccuratePosition.z)
			);
		}
	}
}