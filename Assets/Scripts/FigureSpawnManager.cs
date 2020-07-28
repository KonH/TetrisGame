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

		void OnValidate() {
			Assert.IsNotNull(_field, nameof(_field));
			Assert.IsNotNull(_spawnTarget, nameof(_spawnTarget));
			Assert.IsNotNull(_spawnContainer, nameof(_spawnContainer));
			Assert.AreNotEqual(0, _figures.Length, nameof(_figures));
		}

		public Figure Spawn() {
			var figure   = _figures[Random.Range(0, _figures.Length)];
			var x        = Random.Range(_field.LeftBorder + _margin, _field.RightBorder + 1 - _margin);
			var position = _spawnTarget.position + Vector3.right * x;
			return Instantiate(figure, position, Quaternion.identity, _spawnContainer);
		}
	}
}