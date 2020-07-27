using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace TetrisGame {
	public sealed class FigureSpawnManager : MonoBehaviour {
		[SerializeField]
		FieldManager Field;

		[Tooltip("Initial position of new figures")]
		[SerializeField]
		Transform SpawnTarget;

		[Tooltip("Parent for new figures")]
		[SerializeField]
		Transform SpawnContainer;

		[SerializeField]
		Figure[] Figures;

		[Tooltip("Free space on spawn area sides")]
		[SerializeField]
		int Margin;

		void OnValidate() {
			Assert.IsNotNull(Field, nameof(Field));
			Assert.IsNotNull(SpawnTarget, nameof(SpawnTarget));
			Assert.IsNotNull(SpawnContainer, nameof(SpawnContainer));
			Assert.AreNotEqual(Figures.Length, 0, nameof(Figures));
		}

		public Figure Spawn() {
			var figure    = Figures[Random.Range(0, Figures.Length)];
			var halfWidth = (int) (Field.FieldWidth / 2);
			var x         = Random.Range(-halfWidth + Margin, halfWidth + 1 - Margin);
			var position  = SpawnTarget.position + Vector3.right * x;
			return Instantiate(figure, position, Quaternion.identity, SpawnContainer);
		}
	}
}