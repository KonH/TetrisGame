using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TetrisGame.Service;
using TetrisGame.State;
using UnityEditor;
using UnityEngine;

namespace TetrisGame.Editor {
	public static class SnapshotProcessor {
		static readonly StringBuilder _sb = new StringBuilder();

		[MenuItem("TetrisGame/Snapshot/Process")]
		public static void Process() {
			_sb.Clear();
			try {
				var snapshots = LoadSnapshots();
				_sb.AppendLine($"Load {snapshots.Length} snapshots");

				var initialFitCountGroups = CreateInitialFitGroups(snapshots);
				var initialAvgEntries     = initialFitCountGroups.Average(g => g.Length);
				_sb.AppendLine(
					$"Found fit count groups: {initialFitCountGroups.Length} (avg {initialAvgEntries:F2} entries)");

				var noSpeedUpsFitCountGroups = DropSpeedUpSnapshots(initialFitCountGroups);
				var noSpeedUpsAvgEntries     = noSpeedUpsFitCountGroups.Average(g => g.Length);
				_sb.AppendLine($"Avg entries without speed ups: {noSpeedUpsAvgEntries:F2} entries");

				_sb.AppendLine();
				_sb.AppendLine("Feature extraction:");

				var sources        = CreatePredictionSources(noSpeedUpsFitCountGroups);
				var figureElements = sources.Select(s => s.Elements).ToArray();

				var possibleFigureTypes = ExtractPossibleFigureTypes(figureElements);
				_sb.AppendLine($"Found figure types: {possibleFigureTypes.Length}");

				if ( possibleFigureTypes.Length != 7 ) {
					_sb.AppendLine("Unexpected figure count");
					return;
				}

				var figureTypes = DetectFigureTypes(figureElements, possibleFigureTypes);
				_sb.AppendLine("Figure type distribution:");
				for ( var i = 0; i < possibleFigureTypes.Length; i++ ) {
					var count = figureTypes.Count(t => t == i);
					_sb.AppendLine($"\tType {i}: {(float) count / figureTypes.Length:P}");
				}

				var fieldElements = sources.Select(s => s.Field).ToArray();

				var minX      = fieldElements.Min(e => (e.Length > 0) ? e.Min(p => p.x) : int.MaxValue);
				var maxX      = fieldElements.Max(e => (e.Length > 0) ? e.Max(p => p.x) : int.MinValue);
				var fieldSize = (maxX - minX) + 1;
				_sb.AppendLine($"Field size: {fieldSize}");

				if ( fieldSize != 10 ) {
					_sb.AppendLine("Unexpected field size");
					return;
				}

				var topLines = ExtractTopLines(fieldElements, fieldSize);
				var firstTopLineExample = topLines.First(l => l.Any(e => (e > 0)));
				_sb.AppendLine($"First top line example: {string.Join("; ", firstTopLineExample)}");
				var lastTopLineExample = topLines.Last(l => l.Any(e => (e > 0)));
				_sb.AppendLine($"Last top line example: {string.Join("; ", lastTopLineExample)}");

				var maxHeight = topLines.Max(l => l.Max());
				_sb.AppendLine($"Max line height: {maxHeight}");

				var normalizedTopLines = NormalizeTopLines(topLines, maxHeight);

				_sb.AppendLine();
				_sb.AppendLine("Target initialization:");

				var rawTargets = CreateRawTargets(noSpeedUpsFitCountGroups);

				var targets = CreateTargets(rawTargets);
				_sb.AppendLine($"Max rotation count (initial): {targets.Max(t => t.Rotation)}");

				targets = ReduceRotations(targets);
				_sb.AppendLine($"Max rotation count (after reduce): {targets.Max(t => t.Rotation)}");
				_sb.AppendLine($"Max offset: {targets.Max(t => Math.Abs(t.Offset))}");

				var features = new PredictionFeatures[sources.Length];
				for ( var i = 0; i < features.Length; i++ ) {
					features[i] = new PredictionFeatures(figureTypes[i], normalizedTopLines[i]);
				}

				var dataSet     = CreateDataSet(possibleFigureTypes.Length, fieldSize, features, targets);
				var dataSetPath = "Snapshots/dataset.tsv";
				SaveDataSet(dataSet, dataSetPath);
				_sb.AppendLine($"Dataset saved to '{dataSetPath}'");
			}
			finally {
				var outputPath = "Logs/ProcessSnapshots.txt";
				Debug.Log($"Processing output saved to '{outputPath}'");
				File.WriteAllText(outputPath, _sb.ToString());
			}
		}

		/// <summary>
		/// Load raw snapshots from Snapshots directory
		/// </summary>
		static SnapshotQueue[] LoadSnapshots() {
			return Directory.GetFiles("Snapshots", "*.json")
				.Select(File.ReadAllText)
				.Select(JsonUtility.FromJson<SnapshotQueue>)
				.ToArray();
		}

		/// <summary>
		/// Group snapshots by fit count (all states, leads to combination)
		/// </summary>
		static SnapshotEntry[][] CreateInitialFitGroups(SnapshotQueue[] snapshots) {
			return snapshots
				.Select(s => s.Queue)
				.Select(q => q.GroupBy(e => e.FitCount))
				.SelectMany(group => {
					var allGroupEntries = group.ToArray();
					// Drop last group (it bad for prediction because isn't succeeded)
					var wantedGroupEntries = allGroupEntries.Take(Math.Max(allGroupEntries.Length - 1, 0)).ToArray();
					return wantedGroupEntries.Select(g => g.ToArray());
				})
				.Where(entries => (entries.Length > 0))
				.ToArray();
		}

		/// <summary>
		/// Removes all SpeedUp inputs, it isn't required for prediction
		/// </summary>
		static SnapshotEntry[][] DropSpeedUpSnapshots(SnapshotEntry[][] groups) {
			return groups
				.Select(g => {
					var result = g.Where(e => e.InputState != InputState.SpeedUp).ToArray();
					if ( result.Length > 0 ) {
						return result;
					}
					var firstEntry = g.First();
					var noActionEntry = new SnapshotEntry {
						Field      = firstEntry.Field,
						Figure     = firstEntry.Figure,
						FitCount   = firstEntry.FitCount,
						InputState = InputState.None,
						Origin     = firstEntry.Origin
					};
					return new[] { noActionEntry };
				})
				.ToArray();
		}

		/// <summary>
		/// Collect initial states, it's enough to predict input actions
		/// </summary>
		static PredictionSource[] CreatePredictionSources(SnapshotEntry[][] groups) {
			return groups
				.Select(g => {
					var state = g.First();
					return new PredictionSource(state.Figure.ToArray(), state.Field.ToArray());
				})
				.ToArray();
		}

		/// <summary>
		/// Collect all input actions for each group
		/// </summary>
		static InputState[][] CreateRawTargets(SnapshotEntry[][] groups) {
			return groups
				.Select(g => g.Select(s => s.InputState).ToArray())
				.ToArray();
		}

		/// <summary>
		/// Create prediction targets to represent required actions in two numbers
		/// </summary>
		static PredictionTarget[] CreateTargets(InputState[][] inputs) {
			return inputs
				.Select(state => {
					var leftCount  = state.Count(input => input == InputState.MoveLeft);
					var rightCount = state.Count(input => input == InputState.MoveRight);
					var rotates    = state.Count(input => input == InputState.Rotate);
					return new PredictionTarget(rightCount - leftCount, rotates);
				})
				.ToArray();
		}

		/// <summary>
		/// Rotation higher than 4 is meaningless, because it cyclic
		/// </summary>
		static PredictionTarget[] ReduceRotations(PredictionTarget[] targets) {
			return targets
				.Select(target => {
					while ( target.Rotation >= 4 ) {
						target = new PredictionTarget(target.Offset, target.Rotation - 4);
					}
					return target;
				})
				.ToArray();
		}

		/// <summary>
		/// Extract all figure types from sources
		/// </summary>
		static Vector2[][] ExtractPossibleFigureTypes(Vector2[][] allElements) {
			var elementTypes = new List<Vector2[]>();
			foreach ( var elements in allElements ) {
				var isFound = false;
				foreach ( var elementType in elementTypes ) {
					if ( IsSame(elements, elementType) ) {
						isFound = true;
					}
				}
				if ( !isFound ) {
					elementTypes.Add(elements);
				}
			}
			return elementTypes.ToArray();
		}

		/// <summary>
		/// Detect figure types from sources
		/// </summary>
		static int[] DetectFigureTypes(Vector2[][] allElements, Vector2[][] types) {
			return allElements
				.Select(e => {
					for ( var i = 0; i < types.Length; i++ ) {
						if ( IsSame(e, types[i]) ) {
							return i;
						}
					}
					throw new InvalidOperationException();
				})
				.ToArray();
		}

		static bool IsSame(Vector2[] leftSet, Vector2[] rightSet) {
			if ( leftSet.Length != rightSet.Length ) {
				return false;
			}
			for ( var i = 0; i < leftSet.Length; i++ ) {
				if ( !Mathf.Approximately(leftSet[i].x, rightSet[i].x) ) {
					return false;
				}
				if ( !Mathf.Approximately(leftSet[i].y, rightSet[i].y) ) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Extract normalized to bottom one top element positions
		/// </summary>
		/// <param name="elements"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		static int[][] ExtractTopLines(Vector2Int[][] elements, int size) {
			return elements
				.Select(es => {
					// Create top line with placeholders
					var result = new int[size];
					for ( var i = 0; i < size; i++ ) {
						var x = i;

						var elementsInColumn = es
							.Where(e => e.x == x)
							.Select(e => e.y)
							.ToArray();
						if ( elementsInColumn.Length > 0 ) {
							var topElement = elementsInColumn.Max();
							result[i] = topElement;
						} else {
							result[i] = int.MaxValue;
						}
					}
					// Normalize
					var minValue = result.Min();
					if ( minValue == int.MaxValue ) {
						minValue = 0;
					}
					for ( var i = 0; i < size; i++ ) {
						result[i] = (result[i] == int.MaxValue) ? minValue : (result[i] - minValue);
					}
					return result;
				})
				.ToArray();
		}

		/// <summary>
		/// Make top line values as (0-1)
		/// </summary>
		static float[][] NormalizeTopLines(int[][] topLines, int maxHeight) {
			return topLines
				.Select(e => e.Select(v => (float) v / maxHeight).ToArray())
				.ToArray();
		}

		/// <summary>
		/// Create normalized dataset for prepared data
		/// </summary>
		static (string[] headers, float[][] values) CreateDataSet(
			int figureTypeCount, int fieldSize, PredictionFeatures[] features, PredictionTarget[] targets) {
			var headers = new List<string>();
			for ( var i = 0; i < figureTypeCount; i++ ) {
				headers.Add($"Figure_{i}");
			}
			for ( var i = 0; i < fieldSize; i++ ) {
				headers.Add($"Line_{i}");
			}
			headers.Add("Offset");
			headers.Add("Rotation");
			var allValues = new List<float[]>();
			for ( var i = 0; i < features.Length; i++ ) {
				var feature = features[i];
				var values  = new float[headers.Count];
				values[feature.FigureType] = 1.0f;
				for ( var j = 0; j < fieldSize; j++ ) {
					values[figureTypeCount + j] = feature.NormalizedTopLine[j];
				}
				var target = targets[i];
				values[figureTypeCount + fieldSize]     = target.Offset;
				values[figureTypeCount + fieldSize + 1] = target.Rotation;
				allValues.Add(values);
			}
			return (headers.ToArray(), allValues.ToArray());
		}

		static void SaveDataSet((string[] headers, float[][] values) dataSet, string dataSetPath) {
			var sb = new StringBuilder();
			sb.Append("\t");
			foreach ( var header in dataSet.headers ) {
				sb.Append(header).Append("\t");
			}
			sb.AppendLine();
			for ( var i = 0; i < dataSet.values.Length; i++ ) {
				sb.Append(i).Append("\t");
				var allValues = dataSet.values[i];
				foreach ( var value in allValues ) {
					sb.Append(value).Append("\t");
				}
				sb.AppendLine();
			}
			File.WriteAllText(dataSetPath, sb.ToString());
		}
	}
}