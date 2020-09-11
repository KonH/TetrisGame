using TetrisGame.Service;
using UnityEngine;

namespace TetrisGame.Settings {
	public static class GameLoopSettingsFactory {
		public static GameLoopSettings Create(GameGlobalSettings settings, int randomSeed) {
			return new GameLoopSettings(settings.Width, settings.Height,
				settings.InitialSpeed, settings.LinesToIncrease, settings.IncreaseValue,
				PopulateFigures(settings), settings.ScorePerLines, randomSeed);
		}

		static Vector2Int[][] PopulateFigures(GameGlobalSettings settings) {
			var figures = new Vector2Int[settings.Figures.Count][];
			for ( var i = 0; i < settings.Figures.Count; i++ ) {
				figures[i] = settings.Figures[i].Elements;
			}
			return figures;
		}
	}
}