using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace TetrisGame {
	public sealed class FigureSpawnManager : MonoBehaviour {
		[SerializeField]
		FieldManager _field;

		[Tooltip("Initial position of new figures")]
		[SerializeField]
		Transform _spawnTarget;

		[Tooltip("Parent for new figures")]
		[SerializeField]
		Transform _spawnContainer;

		[SerializeField]
		Figure[] _figures;

		[Tooltip("Free space on spawn area sides")]
		[SerializeField]
		int _margin;

		Figure[] _currentFigures;
		int      _spawnedIndex;
		Figure   _spawnedFigure;

		void Awake() {
			_currentFigures = new Figure[_figures.Length];
		}

		void OnValidate() {
			Assert.IsNotNull(_field, nameof(_field));
			Assert.IsNotNull(_spawnTarget, nameof(_spawnTarget));
			Assert.IsNotNull(_spawnContainer, nameof(_spawnContainer));
			Assert.AreNotEqual(0, _figures.Length, nameof(_figures));
		}

		public Figure Spawn() {
			var x        = Random.Range(_field.LeftBorder + _margin, _field.RightBorder + 1 - _margin);
			var position = _spawnTarget.position + Vector3.right * x;
			_spawnedIndex  = Random.Range(0, _figures.Length);
			_spawnedFigure = GetOrCreate(_spawnedIndex, position);
			return _spawnedFigure;
		}

		Figure GetOrCreate(int index, Vector3 position) {
			var pooledFigure = _currentFigures[index];
			if ( pooledFigure ) {
				pooledFigure.Show(position);
				return pooledFigure;
			}
			var figurePrefab   = _figures[index];
			var figureInstance = Instantiate(figurePrefab, position, Quaternion.identity, _spawnContainer);
			_currentFigures[index] = figureInstance;
			return figureInstance;
		}

		public void Recycle() {
			Assert.IsNotNull(_spawnedFigure);
			_spawnedFigure.Hide();
			_spawnedFigure = null;
		}
	}
}