using System;
using System.Collections.Generic;
using System.Linq;
using TetrisGame.State;

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
			var variantFits = new (InputState[] inputs, long fit)[variants.Length];
			for ( var i = 0; i < variants.Length; i++ ) {
				var variant = variants[i];
				variantFits[i] = (variant, Simulate(gameState, variant));
			}
			var bestFit      = variantFits.Max(w => w.fit);
			var bestVariants = variantFits.Where(w => w.fit == bestFit).ToArray();
			var bestVariant  = bestVariants[_random.Next(0, bestVariants.Length)].inputs;
			return bestVariant;
		}

		InputState[][] GetVariants() {
			var results = new List<InputState[]> { new[] { InputState.None } };

			const int offset = 7;
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

		long Simulate(IReadOnlyGameState initialState, InputState[] inputs) {
			var loop = new CommonGameLoop(_loopSettings, CloneState(initialState));
			PerformSimulation(loop, inputs);
			return CalculateFit(loop.State);
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

		long CalculateFit(IReadOnlyGameState state) {
			var fieldCoeff = GetFieldCoefficient(state.Field);
			return state.Scores - fieldCoeff;
		}

		long GetFieldCoefficient(IReadOnlyFieldState field) {
			var value = 0L;

			var topPositions = new int[field.Width];
			for ( var x = 0; x < field.Width; x++ ) {
				for ( var y = 0; y < field.Height; y++ ) {
					if ( !field.GetState(x, y) ) {
						continue;
					}
					value += (y + 1) * _geneticSettings.FillEntryCoeff;
					if ( topPositions[x] < y ) {
						topPositions[x] = y;
					}
				}
			}

			for ( var i = 0; i < topPositions.Length; i++ ) {
				value += topPositions[i] * _geneticSettings.TopEntryCoeff;
			}

			for ( var x = 0; x < field.Width; x++ ) {
				for ( var y = 0; y < topPositions[x]; y++ ) {
					if ( field.GetState(x, y) ) {
						continue;
					}
					value += (y + 1) * _geneticSettings.HoleEntryCoeff;
				}
			}

			return value;
		}
	}
}