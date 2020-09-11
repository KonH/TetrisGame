namespace TetrisGame.Settings {
	public sealed class GameRuntimeSettings {
		public static GameRuntimeSettings Instance { get; } = new GameRuntimeSettings();

		public bool UseAI;
	}
}