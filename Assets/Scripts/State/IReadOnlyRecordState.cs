using System.Collections.Generic;

namespace TetrisGame.State {
	public interface IReadOnlyRecordState {
		IReadOnlyList<int> Records { get; }
	}
}