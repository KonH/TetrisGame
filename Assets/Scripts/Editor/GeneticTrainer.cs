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
using Random = UnityEngine.Random;

namespace TetrisGame.Editor {
	[CreateAssetMenu]
	public sealed class GeneticTrainer : ScriptableObject {
		public int   GenerationSize;
		public int   SeedCount;
		public float MutationRate;
		public float MutationStep;

		[ContextMenu("Train")]
		public void TrainSingle() {
			Train();
		}

		[ContextMenu("Train (x10)")]
		public void TrainMultiple1() {
			TrainMultiple(10);
		}

		[ContextMenu("Train (x30)")]
		public void TrainMultiple2() {
			TrainMultiple(30);
		}

		[ContextMenu("Train (x100)")]
		public void TrainMultiple3() {
			TrainMultiple(100);
		}

		public void TrainMultiple(int count) {
			for ( var i = 0; i < count; i++ ) {
				Train();
			}
		}

		public void Train() {
			var (generationIndex, settings) = LoadLatestGeneration();
			var logLock     = new object();
			var logFilePath = $"Logs/SettingsTrain_{generationIndex}.log";
			var globalSettings =
				AssetDatabase.LoadAssetAtPath<GameGlobalSettings>("Assets/Settings/GameGlobalSettings.asset");
			var inputs = settings
				.Select(s => {
					var seeds = Enumerable.Range(1, SeedCount).ToArray();
					return (settings: s, seeds);
				})
				.ToArray();
			if ( File.Exists(logFilePath) ) {
				File.Delete(logFilePath);
			}
			File.AppendAllText(logFilePath, $"Train on {inputs.Length} settings");
			var sw      = Stopwatch.StartNew();
			var results = new ConcurrentBag<(GeneticSettings settings, long scores)>();
			Parallel.ForEach(inputs, input => {
				var subResults = new ConcurrentBag<long>();
				Parallel.ForEach(input.seeds, seed => {
					var loopSettings = GameLoopSettingsFactory.Create(globalSettings, seed);
					var loop         = new GeneticGameLoop(loopSettings, input.settings, false);
					while ( !loop.State.Finished ) {
						loop.Update(0.33f);
					}
					subResults.Add(loop.State.Scores);
				});
				results.Add((input.settings, subResults.Sum()));
				lock ( logLock ) {
					File.AppendAllText(logFilePath, $"\nTotal progress: {(float) results.Count / inputs.Length:P}");
				}
			});
			sw.Stop();
			var sb = new StringBuilder();
			var headers = new[] {
				nameof(GeneticSettings.LinesCleared),
				nameof(GeneticSettings.WeightedHeight),
				nameof(GeneticSettings.CumulativeHeight),
				nameof(GeneticSettings.RelativeHeight),
				nameof(GeneticSettings.Holes),
				nameof(GeneticSettings.Roughness),
				"Scores"
			};
			sb.AppendLine(string.Join(", ", headers));

			var orderedResults = results.OrderByDescending(r => r.scores).ToArray();
			foreach ( var result in orderedResults ) {
				var values = new[] {
					result.settings.LinesCleared,
					result.settings.WeightedHeight,
					result.settings.CumulativeHeight,
					result.settings.RelativeHeight,
					result.settings.Holes,
					result.settings.Roughness,
					result.scores,
				};
				sb.AppendLine(string.Join(", ", values));
			}

			var bestScores   = orderedResults.First().scores;
			var bestSettings = orderedResults.Where(r => r.scores == bestScores).Select(r => r.settings).ToArray();
			File.AppendAllText(logFilePath, $"\nBest result: {(float) bestScores / SeedCount:F}");
			WriteInterval(logFilePath, bestSettings, nameof(GeneticSettings.LinesCleared), s => s.LinesCleared);
			WriteInterval(logFilePath, bestSettings, nameof(GeneticSettings.WeightedHeight), s => s.WeightedHeight);
			WriteInterval(logFilePath, bestSettings, nameof(GeneticSettings.CumulativeHeight), s => s.CumulativeHeight);
			WriteInterval(logFilePath, bestSettings, nameof(GeneticSettings.RelativeHeight), s => s.RelativeHeight);
			WriteInterval(logFilePath, bestSettings, nameof(GeneticSettings.Holes), s => s.Holes);
			WriteInterval(logFilePath, bestSettings, nameof(GeneticSettings.Roughness), s => s.Roughness);
			var resultFilePath = $"Logs/SettingsTrain_{generationIndex}.csv";
			File.WriteAllText(resultFilePath, sb.ToString());
			Debug.Log($"Generation: {generationIndex}" +
			          $"\nBest result: {(float) bestScores / SeedCount:F}\n" +
			          $"Log saved into '{logFilePath}', " +
			          $"result saved into '{resultFilePath}', elapsed time: {sw.Elapsed.TotalMinutes:F2} min");
			var geneticSettings =
				AssetDatabase.LoadAssetAtPath<GameGeneticSettings>("Assets/Settings/GameGeneticSettings.asset");
			bestSettings[0].UseDebugging = geneticSettings.Settings.UseDebugging;
			geneticSettings.Settings = bestSettings[0];
			EditorUtility.SetDirty(geneticSettings);
			AssetDatabase.SaveAssets();
		}

		(int generationIndex, GeneticSettings[]) LoadLatestGeneration() {
			var generations    = Directory.GetFiles("Logs");
			var maxGeneration  = 0;
			var generationName = string.Empty;
			foreach ( var fileName in generations ) {
				var parts      = fileName.Split('_', '.');
				if ( (parts.Length != 3) || (parts[2] != "csv") ) {
					continue;
				}
				var generation = int.Parse(parts[1]);
				if ( generation >= maxGeneration ) {
					maxGeneration  = generation;
					generationName = fileName;
				}
			}
			if ( !string.IsNullOrEmpty(generationName) ) {
				return (maxGeneration + 1, ProcessGeneration(generationName));
			}
			var randomSettings = new GeneticSettings[GenerationSize];
			for ( var i = 0; i < GenerationSize; i++ ) {
				randomSettings[i] = new GeneticSettings(
					Random.Range(-1.0f, 1.0f),
					Random.Range(-1.0f, 1.0f),
					Random.Range(-1.0f, 1.0f),
					Random.Range(-1.0f, 1.0f),
					Random.Range(-1.0f, 1.0f),
					Random.Range(-1.0f, 1.0f),
					false);
			}
			return (0, randomSettings);
		}

		GeneticSettings[] ProcessGeneration(string sourceFileName) {
			var previousGeneration   = ReadGeneration(sourceFileName);
			var betterHalfGeneration = previousGeneration.Take(previousGeneration.Length / 2).ToArray();
			var newGeneration        = new List<GeneticSettings> { betterHalfGeneration[0] };
			while ( newGeneration.Count < GenerationSize ) {
				var child = Cross(betterHalfGeneration);
				newGeneration.Add(Mutate(child));
			}
			return newGeneration.ToArray();
		}

		GeneticSettings[] ReadGeneration(string sourceFileName) {
			var contents    = File.ReadAllLines(sourceFileName).Skip(1);
			var allSettings = new List<GeneticSettings>();
			foreach ( var content in contents ) {
				var parts = content.Split(',');
				if ( parts.Length < 6 ) {
					continue;
				}
				allSettings.Add(new GeneticSettings(
					float.Parse(parts[0]),
					float.Parse(parts[1]),
					float.Parse(parts[2]),
					float.Parse(parts[3]),
					float.Parse(parts[4]),
					float.Parse(parts[5]),
					false
				));
			}
			return allSettings.ToArray();
		}

		GeneticSettings Cross(GeneticSettings[] parents) {
			var leftParent  = parents[Random.Range(0, parents.Length)];
			var rightParent = parents[Random.Range(0, parents.Length)];
			return Cross(leftParent, rightParent);
		}

		GeneticSettings Cross(GeneticSettings leftParent, GeneticSettings rightParent) {
			return new GeneticSettings(
				Cross(leftParent.LinesCleared, rightParent.LinesCleared),
				Cross(leftParent.WeightedHeight, rightParent.WeightedHeight),
				Cross(leftParent.CumulativeHeight, rightParent.CumulativeHeight),
				Cross(leftParent.RelativeHeight, rightParent.RelativeHeight),
				Cross(leftParent.Holes, rightParent.Holes),
				Cross(leftParent.Roughness, rightParent.Roughness),
				false);
		}

		GeneticSettings Mutate(GeneticSettings source) {
			return new GeneticSettings(
				Mutate(source.LinesCleared),
				Mutate(source.WeightedHeight),
				Mutate(source.CumulativeHeight),
				Mutate(source.RelativeHeight),
				Mutate(source.Holes),
				Mutate(source.Roughness),
				false);
		}

		float Mutate(float source) {
			if ( Random.value < MutationRate ) {
				return source + Random.value * MutationStep * 2 - MutationStep;
			}
			return source;
		}

		float Cross(float left, float right) {
			return (Random.value > 0.5f) ? left : right;
		}

		void WriteInterval
		(string logFilePath, GeneticSettings[] settings, string intervalName,
			Func<GeneticSettings, float> selector) {
			var min = settings.Select(selector).Min();
			var max = settings.Select(selector).Max();
			File.AppendAllText(logFilePath, $"\nBest {intervalName}: {min}-{max}");
		}
	}
}