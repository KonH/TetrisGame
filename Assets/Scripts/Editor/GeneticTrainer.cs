using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisGame.Service;
using TetrisGame.Settings;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace TetrisGame.Editor {
	[CreateAssetMenu]
	public sealed class GeneticTrainer : ScriptableObject {
		public int MinFillEntry;
		public int MaxFillEntry;
		public int MinTopEntry;
		public int MaxTopEntry;
		public int MinHoleEntry;
		public int MaxHoleEntry;
		public int Step;

		[ContextMenu("DirectTrain")]
		public void DirectTrain() {
			var seedCount   = 30;
			var logLock     = new object();
			var logFilePath = "Logs/DirectSettingsTrain.log";
			var globalSettings =
				AssetDatabase.LoadAssetAtPath<GameGlobalSettings>("Assets/Settings/GameGlobalSettings.asset");
			var allSettings  = GetAllSettings();
			var inputs = allSettings
				.Select(settings => {
					var seeds = Enumerable.Range(1, seedCount).ToArray();
					return (settings, seeds);
				})
				.ToArray();
			if ( File.Exists(logFilePath) ) {
				File.Delete(logFilePath);
			}
			File.AppendAllText(logFilePath, $"Train on {inputs.Length} settings");
			var sw      = Stopwatch.StartNew();
			var results = new ConcurrentBag<(GeneticSettings settings, long scores)>();
			Parallel.ForEach(inputs, input => {
				var settings   = input.settings;
				var subResults = new ConcurrentBag<long>();
				Parallel.ForEach(input.seeds, seed => {
					var loopSettings = GameLoopSettingsFactory.Create(globalSettings, seed);
					var loop         = new GeneticGameLoop(loopSettings, settings);
					while ( !loop.State.Finished ) {
						loop.Update(0.33f);
					}
					subResults.Add(loop.State.Scores);
				});
				results.Add((settings, subResults.Sum()));
				lock ( logLock ) {
					File.AppendAllText(logFilePath, $"\nTotal progress: {(float)results.Count / inputs.Length:P}");
				}
			});
			sw.Stop();
			var sb = new StringBuilder();
			var headers = new[] {
				nameof(GeneticSettings.FillEntryCoeff),
				nameof(GeneticSettings.TopEntryCoeff),
				nameof(GeneticSettings.HoleEntryCoeff),
				"Scores"
			};
			sb.AppendLine(string.Join(", ", headers));

			var orderedResults = results.OrderByDescending(r => r.scores).ToArray();
			foreach ( var result in orderedResults ) {
				var values = new[] {
					result.settings.FillEntryCoeff,
					result.settings.TopEntryCoeff,
					result.settings.HoleEntryCoeff,
					result.scores,
				};
				sb.AppendLine(string.Join(", ", values));
			}

			var bestScores     = orderedResults.First().scores;
			var bestSettings   = orderedResults.Where(r => r.scores == bestScores).Select(r => r.settings).ToArray();
			File.AppendAllText(logFilePath, $"\nBest result: {(float)bestScores / seedCount:F}");
			WriteInterval(logFilePath, bestSettings, nameof(GeneticSettings.FillEntryCoeff), s => s.FillEntryCoeff);
			WriteInterval(logFilePath, bestSettings, nameof(GeneticSettings.TopEntryCoeff), s => s.TopEntryCoeff);
			WriteInterval(logFilePath, bestSettings, nameof(GeneticSettings.HoleEntryCoeff), s => s.HoleEntryCoeff);
			var resultFilePath = "Logs/DirectSettingsTrain.csv";
			File.WriteAllText(resultFilePath, sb.ToString());
			Debug.Log($"Best result: {(float)bestScores / seedCount:F}\n" +
			          $"Log saved into '{logFilePath}', " +
			          $"result saved into '{resultFilePath}', elapsed time: {sw.Elapsed.TotalMinutes:F2} min");
		}

		GeneticSettings[] GetAllSettings() {
			var fillEntryCoeffs = GetStepRange(MinFillEntry, MaxFillEntry, Step);
			var topEntryCoeffs  = GetStepRange(MinTopEntry, MaxTopEntry, Step);
			var holeEntryCoeffs = GetStepRange(MinHoleEntry, MaxHoleEntry, Step);
			var settings        = new List<GeneticSettings>();
			foreach ( var fill in fillEntryCoeffs ) {
				foreach ( var top in topEntryCoeffs ) {
					foreach ( var hole in holeEntryCoeffs ) {
						settings.Add(new GeneticSettings(
							fill,
							top,
							hole
						));
					}
				}
			}
			return settings.ToArray();
		}

		int[] GetStepRange(int min, int max, int step) {
			var result  = new List<int>();
			if ( min == max ) {
				return new[] { min };
			}
			for ( var current = min; current <= max; current += step ) {
				result.Add(current);
			}
			return result.ToArray();
		}

		void WriteInterval
			(string logFilePath, GeneticSettings[] settings, string name, Func<GeneticSettings, int> selector) {
			var min = settings.Select(selector).Min();
			var max = settings.Select(selector).Max();
			File.AppendAllText(logFilePath, $"\nBest {name}: {min}-{max}");
		}
	}
}