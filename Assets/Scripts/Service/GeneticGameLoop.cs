using JetBrains.Annotations;
using TetrisGame.State;

namespace TetrisGame.Service {
	public sealed class GeneticGameLoop : IGameLoop {
		readonly GameState      _state;
		readonly CommonGameLoop _loop;

		readonly GeneticPlayer _geneticPlayer;
		readonly RecordWriter  _recordWriter;

		[CanBeNull]
		readonly GeneticDebugger _debugger;

		public IReadOnlyGameState State => _loop.State;

		public GeneticGameLoop(
			GameLoopSettings loopSettings, GeneticSettings geneticSettings, bool useRecords) {
			_state         = new GameState(loopSettings.Width, loopSettings.Height, loopSettings.InitialSpeed);
			_loop          = new CommonGameLoop(loopSettings, _state);
			_debugger      = geneticSettings.UseDebugging ? new GeneticDebugger() : null;
			_geneticPlayer = new GeneticPlayer(loopSettings, geneticSettings, _debugger);

			if ( !useRecords ) {
				return;
			}
			var recordReader = new RecordReader();
			recordReader.Read(_state.Records);
			_recordWriter = new RecordWriter();
		}

		public void Update(float dt) {
			if ( _loop.PreUpdate() ) {
				_geneticPlayer.Update(_state);
				_loop.PostUpdate(dt);
			} else {
				_debugger?.Dispose();
				_recordWriter?.Write(_state.Records, new RecordUnit(_state.Scores, true));
			}
		}

		public void UpdateInput(InputState input) {}
	}
}