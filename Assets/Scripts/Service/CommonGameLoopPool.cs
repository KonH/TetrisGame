using TetrisGame.State;

namespace TetrisGame.Service {
	sealed class CommonGameLoopPool : Pool<CommonGameLoop> {
		readonly GameLoopSettings _loopSettings;

		public CommonGameLoopPool(GameLoopSettings loopSettings) {
			_loopSettings = loopSettings;
		}

		public CommonGameLoop Get(GameState state) {
			if ( Elements.Count > 0 ) {
				var loop = Elements.Dequeue();
				loop.Reset(state);
				return loop;
			}
			return new CommonGameLoop(_loopSettings, state);
		}
	}
}