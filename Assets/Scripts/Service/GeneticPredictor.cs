using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TetrisGame.State;
using UnityEngine;
using Random = System.Random;

namespace TetrisGame.Service {
	public sealed class GeneticPredictor {
		static readonly InputState[][] _allVariants = GetAllVariants();

		readonly GameLoopSettings _loopSettings;
		readonly GeneticSettings  _geneticSettings;

		[CanBeNull]
		readonly GeneticDebugger _debugger;

		readonly Random _random;

		public GeneticPredictor(
			GameLoopSettings loopSettings, GeneticSettings geneticSettings, [CanBeNull] GeneticDebugger debugger) {
			_loopSettings    = loopSettings;
			_geneticSettings = geneticSettings;
			_debugger        = debugger;
			_random          = new Random(loopSettings.RandomSeed);
		}

		public InputState[] GetBestInputs(IReadOnlyGameState gameState) {
			_debugger?.BeforeBestInputSelection(gameState);
			var variants = _allVariants;
			var variantFits = new (InputState[] inputs, IReadOnlyGameState state, float fit)[variants.Length];
			_debugger?.BeforeAllSimulations();
			for ( var i = 0; i < variants.Length; i++ ) {
				var variant = variants[i];
				var (simulatedState, fit) = Simulate(gameState, variant);
				variantFits[i] = (variant, simulatedState, fit);
				_debugger?.AfterSimulation(i, variant, gameState, simulatedState);
			}
			_debugger?.AfterAllSimulations();
			var bestFit       = variantFits.Max(w => w.fit);
			var bestVariants  = variantFits.Where(w => Mathf.Approximately(w.fit, bestFit)).ToArray();
			var otherVariants = variantFits.Where(w => !Mathf.Approximately(w.fit, bestFit)).ToArray();
			var bestIndex     = _random.Next(0, bestVariants.Length);
			var bestVariant   = bestVariants[bestIndex];
			_debugger?
				.AfterBestInputSelection(bestFit, gameState, bestVariants, bestVariant, otherVariants, bestIndex);
			return bestVariant.inputs;
		}

		static InputState[][] GetAllVariants() {
			var results = new List<InputState[]> { new[] { InputState.None } };

			const int offset = 5;
			for ( var i = 1; i <= offset; i++ ) {
				results.Add(FillMovement(i, InputState.MoveLeft));
				results.Add(FillMovement(i, InputState.MoveRight));
			}

			var count = results.Count;
			const int maxRotations = 3;
			for ( var rotations = 1; rotations <= maxRotations; rotations++ ) {
				for ( var i = 0; i < count; i++ ) {
					var rawInputs          = results[i];
					var inputWithRotations = rawInputs.ToList();
					for ( var j = 0; j < rotations; j++ ) {
						inputWithRotations.Insert(0, InputState.Rotate);
					}
					results.Add(inputWithRotations.ToArray());
				}
			}
			return results.ToArray();
		}

		static InputState[] FillMovement(int count, InputState state) {
			var result = new InputState[count];
			for ( var j = 0; j < count; j++ ) {
				result[j] = state;
			}
			return result;
		}

		(IReadOnlyGameState state, float fit) Simulate(IReadOnlyGameState initialState, InputState[] inputs) {
			var state = CloneState(initialState);
			var loop  = new CommonGameLoop(_loopSettings, state);
			PerformSimulation(loop, inputs);
			return (state, CalculateFit(initialState, loop.State));
		}

		GameState CloneState(IReadOnlyGameState oldState) {
			var settings  = _loopSettings;
			var state     = new GameState(settings.Width, settings.Height, settings.InitialSpeed);
			state.ClearedLines = oldState.ClearedLines;
			var oldFigure = oldState.Figure;
			var newFigure = state.Figure;
			newFigure.Elements.AddRange(oldFigure.Elements);
			newFigure.Origin = oldFigure.Origin;
			var oldField  = oldState.Field;
			var newField  = state.Field;
			for ( var x = 0; x < oldField.Width; x++ ) {
				for ( var y = 0; y < oldField.Height; y++ ) {
					newField.Field[x, y] = oldField.GetState(x, y);
				}
			}
			return state;
		}

		void PerformSimulation(IGameLoop loop, InputState[] inputs) {
			var inputIndex = 0;
			var isFinished = false;
			while ( !isFinished ) {
				loop.Update(0.33f);
				if ( inputIndex < inputs.Length ) {
					loop.UpdateInput(inputs[inputIndex]);
				}
				inputIndex++;
				isFinished = loop.State.Finished || (loop.State.FitCount > 0);
			}
		}

		float CalculateFit(IReadOnlyGameState oldState, IReadOnlyGameState state) {
			var linesCleared = (state.ClearedLines - oldState.ClearedLines);
			var heights      = new int[state.Field.Width];
			for ( var x = 0; x < state.Field.Width; x++ ) {
				for ( var y = 0; y < state.Field.Height; y++ ) {
					if ( state.Field.GetState(x, y) && (y > heights[x]) ) {
						heights[x] = y;
					}
				}
			}
			var weightedHeight = heights.Max();
			var cumulativeHeight = heights.Sum();
			var relativeHeight = weightedHeight - heights.Min();
			var holes = 0;
			for ( var x = 0; x < state.Field.Width; x++ ) {
				for ( var y = 0; y < state.Field.Height; y++ ) {
					if ( !state.Field.GetState(x, y) && (y < heights[x]) ) {
						holes++;
					}
				}
			}
			var roughness = 0;
			for ( var i = 1; i < heights.Length; i++ ) {
				roughness += (heights[i] - heights[i - 1]);
			}
			var result =
				linesCleared * _geneticSettings.LinesCleared +
				weightedHeight * _geneticSettings.WeightedHeight +
				cumulativeHeight * _geneticSettings.CumulativeHeight +
				relativeHeight * _geneticSettings.RelativeHeight +
				holes * _geneticSettings.Holes +
				roughness * _geneticSettings.Roughness;
			_debugger?.WriteFitSummary(
				linesCleared, weightedHeight, cumulativeHeight, relativeHeight, holes, roughness, result);
			return result;
		}
	}
}