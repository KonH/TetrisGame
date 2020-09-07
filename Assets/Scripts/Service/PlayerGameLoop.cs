using TetrisGame.State;

namespace TetrisGame.Service {
	public sealed class PlayerGameLoop : IGameLoop {
		readonly GameState      _state;
		readonly CommonGameLoop _loop;

		readonly RecordWriter _recordWriter;

		public IReadOnlyGameState State => _state;

		public PlayerGameLoop(GameLoopSettings settings) {
			_loop = new CommonGameLoop(settings, _state);
			var recordReader = new RecordReader();
			recordReader.Read(_state.Records);
		}

		public void Update(float dt) {
			if ( _loop.PreUpdate() ) {
				_loop.PostUpdate(dt);
			} else {
				_recordWriter.Write(_state.Records, new RecordUnit(_state.Scores, true));
			}
		}

		public void UpdateInput(InputState input) =>
			_loop.UpdateInput(input);
	}
}