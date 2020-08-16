using TetrisGame.State;

namespace TetrisGame.Service {
	/// <summary>
	/// Update speed each collapsed linesToIncrease by increasedValue
	/// </summary>
	public sealed class SpeedController {
		readonly int   _linesToIncrease;
		readonly float _increaseValue;

		public SpeedController(int linesToIncrease, float increaseValue) {
			_linesToIncrease = linesToIncrease;
			_increaseValue   = increaseValue;
		}

		public void ApplyLines(SpeedState state, int count) {
			state.CurrentLines += count;
			var tmp = state.CurrentLines - _linesToIncrease;
			if ( tmp > 0 ) {
				state.CurrentLines = tmp;
				state.Current += _increaseValue;
				state.Level++;
			}
		}
	}
}