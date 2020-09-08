using System.Collections.Generic;
using System.Linq;
using TetrisGame.State;
using UnityEngine;
using Random = System.Random;

namespace TetrisGame.Service {
	public sealed class GeneticPredictor {
		readonly GameLoopSettings _loopSettings;
		readonly GeneticSettings  _geneticSettings;

		readonly Random _random;

		public GeneticPredictor(GameLoopSettings loopSettings, GeneticSettings geneticSettings) {
			_loopSettings    = loopSettings;
			_geneticSettings = geneticSettings;
			_random          = new Random(loopSettings.RandomSeed);
		}

		public InputState[] GetBestInputs(IReadOnlyGameState gameState) {
			var variants = GetVariants();
			var variantFits = new (InputState[] inputs, float fit)[variants.Length];
			for ( var i = 0; i < variants.Length; i++ ) {
				var variant = variants[i];
				variantFits[i] = (variant, Simulate(gameState, variant));
			}
			var bestFit      = variantFits.Max(w => w.fit);
			var bestVariants = variantFits.Where(w => Mathf.Approximately(w.fit, bestFit)).ToArray();
			var bestVariant  = bestVariants[_random.Next(0, bestVariants.Length)].inputs;
			return bestVariant;
		}

		InputState[][] GetVariants() {
			var results = new List<InputState[]> { new[] { InputState.None } };

			const int offset = 5;
			for ( var i = 1; i < offset; i++ ) {
				results.Add(FillMovement(i, InputState.MoveLeft));
				results.Add(FillMovement(i, InputState.MoveRight));
			}

			var count = results.Count;
			const int rotations = 3;
			for ( var i = 0; i < count; i++ ) {
				var rawInputs = results[i];
				var inputWithRotations = rawInputs.ToList();
				for ( var j = 0; j < rotations; j++ ) {
					inputWithRotations.Insert(0, InputState.Rotate);
				}
				results.Add(inputWithRotations.ToArray());
			}
			return results.ToArray();
		}

		InputState[] FillMovement(int count, InputState state) {
			var result = new InputState[count];
			for ( var j = 0; j < count; j++ ) {
				result[j] = state;
			}
			return result;
		}

		float Simulate(IReadOnlyGameState initialState, InputState[] inputs) {
			var loop = new CommonGameLoop(_loopSettings, CloneState(initialState));
			PerformSimulation(loop, inputs);
			return CalculateFit(initialState, loop.State);
		}

		GameState CloneState(IReadOnlyGameState oldState) {
			var settings  = _loopSettings;
			var state     = new GameState(settings.Width, settings.Height, settings.InitialSpeed);
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
			return
				linesCleared * _geneticSettings.LinesCleared +
				weightedHeight * _geneticSettings.WeightedHeight +
				cumulativeHeight * _geneticSettings.CumulativeHeight +
				relativeHeight * _geneticSettings.RelativeHeight +
				holes * _geneticSettings.Holes +
				roughness * _geneticSettings.Roughness;
		}
	}
}