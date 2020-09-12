using TetrisGame.State;

namespace TetrisGame.Service {
	public sealed class PlayerGameLoop : IGameLoop {
		readonly GameState      _state;
		readonly CommonGameLoop _loop;

		readonly RecordWriter _recordWriter;

		public IReadOnlyGameState State => _state;

		public PlayerGameLoop(GameLoopSettings settings) {
			_state = new GameState(settings.Width, settings.Height, settings.InitialSpeed);
			_loop  = new CommonGameLoop(settings, _state);
			var recordReader = new RecordReader();
			recordReader.Read(_state.Records);
			_recordWriter = new RecordWriter();
		}

		public void Update(float dt) {
			if ( _loop.PreUpdate() ) {
				_loop.PostUpdate(dt);
			} else {
				_recordWriter.Write(_state.Records, new RecordUnit(_state.Scores, false));
			}
		}

		public void UpdateInput(InputState input) =>
			_loop.UpdateInput(input);
	}
}