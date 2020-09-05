using TetrisGame.State;

namespace TetrisGame.Service {
	public sealed class GeneticGameLoop : IGameLoop {
		readonly GameState      _state;
		readonly CommonGameLoop _loop;

		readonly GeneticPlayer _geneticPlayer;

		public IReadOnlyGameState State => _loop.State;

		public GeneticGameLoop(GameLoopSettings loopSettings, GeneticSettings geneticSettings) {
			_state         = new GameState(loopSettings.Width, loopSettings.Height, loopSettings.InitialSpeed);
			_loop          = new CommonGameLoop(loopSettings, _state);
			_geneticPlayer = new GeneticPlayer(loopSettings, geneticSettings);
		}

		public void Update(float dt) {
			if ( !_loop.PreUpdate() ) {
				return;
			}
			_geneticPlayer.Update(_state);
			_loop.PostUpdate(dt);
		}

		public void UpdateInput(InputState input) {}
	}
}