using System.Collections.Generic;
using JetBrains.Annotations;
using TetrisGame.State;
using UnityEngine;
using Random = System.Random;

namespace TetrisGame.Service {
	public sealed class GeneticPredictor {
		static readonly InputState[][] _allVariants = GetAllVariants();

		readonly GeneticSettings  _geneticSettings;

		[CanBeNull]
		readonly GeneticDebugger _debugger;

		readonly Random _random;

		readonly VariantFit[] _variantFits = new VariantFit[_allVariants.Length];

		readonly List<VariantFit> _bestVariants  = new List<VariantFit>(_allVariants.Length);
		readonly List<VariantFit> _otherVariants = new List<VariantFit>(_allVariants.Length);

		readonly GameStatePool      _gameStatePool;
		readonly CommonGameLoopPool _gameLoopPool;

		readonly int[] _heights;

		public GeneticPredictor(
			GameLoopSettings loopSettings, GeneticSettings geneticSettings, [CanBeNull] GeneticDebugger debugger) {
			_geneticSettings = geneticSettings;
			_debugger        = debugger;
			_random          = new Random(loopSettings.RandomSeed);
			_gameStatePool   = new GameStatePool(loopSettings.Width, loopSettings.Height, loopSettings.InitialSpeed);
			_gameLoopPool    = new CommonGameLoopPool(loopSettings);
			_heights         = new int[loopSettings.Width];
		}

		public InputState[] GetBestInputs(IReadOnlyGameState gameState) {
			_debugger?.BeforeBestInputSelection(gameState);
			var variants = _allVariants;
			var variantFits = _variantFits;
			_debugger?.BeforeAllSimulations();
			for ( var i = 0; i < variants.Length; i++ ) {
				var variant = variants[i];
				_debugger?.BeforeSimulation(i, variant);
				var (simulatedState, fit) = Simulate(gameState, variant);
				variantFits[i] = new VariantFit(variant, simulatedState, fit);
				_debugger?.AfterSimulation(i, gameState, simulatedState);
			}
			_debugger?.AfterAllSimulations();
			var bestFit       = GetMaxFit(variantFits);
			var bestVariants  = GetBestVariants(variantFits, bestFit);
			var bestIndex     = _random.Next(0, bestVariants.Count);
			var bestVariant   = bestVariants[bestIndex];
			if ( _debugger != null ) {
				var otherVariants = GetOtherVariants(variantFits, bestFit);
				_debugger?
					.AfterBestInputSelection(bestFit, gameState, bestVariants, bestVariant, otherVariants, bestIndex);
			}
			for ( var i = 0; i < variants.Length; i++ ) {
				_gameStatePool.Release(variantFits[i].State);
			}
			return bestVariant.Inputs;
		}

		float GetMaxFit(VariantFit[] variantFits) {
			var max = float.MinValue;
			for ( var i = 0; i < variantFits.Length; i++ ) {
				var fit = variantFits[i].Fit;
				if ( fit > max ) {
					max = fit;
				}
			}
			return max;
		}

		List<VariantFit> GetBestVariants(VariantFit[] variantFits, float max) {
			_bestVariants.Clear();
			var results = _bestVariants;
			for ( var i = 0; i < variantFits.Length; i++ ) {
				var variant = variantFits[i];
				if ( Mathf.Approximately(variant.Fit, max) ) {
					results.Add(variant);
				}
			}
			return results;
		}

		List<VariantFit> GetOtherVariants(VariantFit[] variantFits, float max) {
			_otherVariants.Clear();
			var results = _otherVariants;
			for ( var i = 0; i < variantFits.Length; i++ ) {
				var variant = variantFits[i];
				if ( !Mathf.Approximately(variant.Fit, max) ) {
					results.Add(variant);
				}
			}
			return results;
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
					var inputWithRotations = new List<InputState>(rawInputs);
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

		(GameState state, float fit) Simulate(IReadOnlyGameState initialState, InputState[] inputs) {
			var state = _gameStatePool.Clone(initialState);
			var loop  = _gameLoopPool.Get(state);
			PerformSimulation(initialState, loop, inputs);
			_gameLoopPool.Release(loop);
			return (state, CalculateFit(initialState, state));
		}

		void PerformSimulation(IReadOnlyGameState initialState, IGameLoop loop, InputState[] inputs) {
			var inputIndex = 0;
			var isFinished = false;
			while ( !isFinished ) {
				loop.Update(0.33f);
				if ( inputIndex < inputs.Length ) {
					loop.UpdateInput(inputs[inputIndex]);
				}
				inputIndex++;
				isFinished = loop.State.Finished || (loop.State.FitCount > initialState.FitCount);
			}
		}

		float CalculateFit(IReadOnlyGameState oldState, IReadOnlyGameState state) {
			var linesCleared = (state.ClearedLines - oldState.ClearedLines);
			var heights      = _heights;
			for ( var i = 0; i < heights.Length; i++ ) {
				heights[i] = 0;
			}
			for ( var x = 0; x < state.Field.Width; x++ ) {
				for ( var y = 0; y < state.Field.Height; y++ ) {
					if ( state.Field.GetStateUnsafe(x, y) && (y > heights[x]) ) {
						heights[x] = y;
					}
				}
			}
			var weightedHeight = Max(heights);
			var cumulativeHeight = Sum(heights);
			var relativeHeight = weightedHeight - Min(heights);
			var holes = 0;
			for ( var x = 0; x < state.Field.Width; x++ ) {
				for ( var y = 0; y < state.Field.Height; y++ ) {
					if ( !state.Field.GetStateUnsafe(x, y) && (y < heights[x]) ) {
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

		int Min(int[] values) {
			var min = int.MaxValue;
			for ( var i = 0; i < values.Length; i++ ) {
				var current = values[i];
				if ( current < min ) {
					min = current;
				}
			}
			return min;
		}

		int Max(int[] values) {
			var max = int.MinValue;
			for ( var i = 0; i < values.Length; i++ ) {
				var current = values[i];
				if ( current > max ) {
					max = current;
				}
			}
			return max;
		}

		int Sum(int[] values) {
			var sum = 0;
			for ( var i = 0; i < values.Length; i++ ) {
				sum += values[i];
			}
			return sum;
		}
	}
}