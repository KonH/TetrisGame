namespace TetrisGame.State {
	public readonly struct RecordUnit {
		public readonly int  Scores;
		public readonly bool IsAI;

		public RecordUnit(int scores, bool isAI) {
			Scores = scores;
			IsAI   = isAI;
		}

		public static bool TryParse(string entry, out RecordUnit unit) {
			unit = default;
			var parts = entry.Split(':');
			if ( parts.Length < 2 ) {
				return false;
			}
			if ( !int.TryParse(parts[0], out var scores) ) {
				return false;
			}
			var isAi = (parts[1] == "AI");
			unit = new RecordUnit(scores, isAi);
			return true;
		}

		public override string ToString() {
			return $"{(IsAI ? "AI" : "YOU")}:{Scores}";
		}
	}
}