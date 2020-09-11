using System.Collections.Generic;

namespace TetrisGame.Service {
	abstract class Pool<T> {
		protected readonly Queue<T> Elements = new Queue<T>();

		public void Release(T element) {
			Elements.Enqueue(element);
		}
	}
}