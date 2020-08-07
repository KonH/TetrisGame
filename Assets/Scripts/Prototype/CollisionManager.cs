using System.Collections.Generic;
using UnityEngine;

namespace TetrisGame {
	public sealed class CollisionManager : MonoBehaviour {
		public bool HasPredictedCollisions(Figure figure, IReadOnlyCollection<Vector3> elements) {
			// Non-optimal but fine for small scale
			foreach ( var element in figure.Elements ) {
				var position = element.transform.position + figure.ScheduledDirection;
				foreach ( var other in elements ) {
					var sqrMagnitude = (other - position).sqrMagnitude;
					if ( sqrMagnitude < 0.95f ) {
						return true;
					}
				}
			}
			return false;
		}
	}
}