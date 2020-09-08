using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using TetrisGame.State;
using UnityEngine;

namespace TetrisGame.Service {
	public sealed class GeneticDebugger : IDisposable {
		readonly string _filePath;

		readonly StringBuilder _content = new StringBuilder();
		readonly Stack<string> _tags    = new Stack<string>();

		public GeneticDebugger() {
			var sessionName = Guid.NewGuid().ToString();
			_filePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"{sessionName}.html");
			OpenTag("html");
			OpenTag("head");
			OpenTag("title");
			Write(sessionName);
			CloseTag();
			OpenTag("style");
			Write("p { margin-top: 0.5em; margin-bottom: 0.5em; }");
			Write("td, th { border: 1px solid black; }");
			Write("table { border-collapse: collapse; }");
			Write(".variant-table { table-layout: fixed; width: 100%; font-size: 0.75em; }");
			Write(".variant-info { width: 11%; }");
			Write(".state { width: 16em; height: 32em; }");
			Write(".state-small { width: 8em; height: 16em; vertical-align: baseline }");
			Write(".figure { background-color: lightgray }");
			Write(".field { background-color: gray }");
			Write(".best { background-color: green }");
			Write(".nice { background-color: yellow }");
			Write(".bad { background-color: red }");
			Write(".empty { background-color: white }");
			CloseTag();
			CloseTag();
			OpenTag("body");
		}

		public void OpenTag(string name, string attributes = null) {
			_tags.Push(name);
			_content.Append("<").Append(name);
			if ( !string.IsNullOrEmpty(attributes) ) {
				_content.Append(" ").Append(attributes);
			}
			_content.Append(">");
		}

		public void CloseTag() {
			_content.Append("</").Append(_tags.Pop()).Append(">");
		}

		public void Write(string content) {
			_content.Append(content);
		}

		public void Write(string tag, string content) {
			OpenTag(tag);
			Write(content);
			CloseTag();
		}

		public void WriteFinalState(
			IReadOnlyGameState state,
			IReadOnlyGameState bestVariant, IReadOnlyGameState[] bestVariants, IReadOnlyGameState[] otherVariants) {
			OpenTag("table", "class=\"state\"");
			for ( var y = state.Field.Height; y >= 0; y-- ) {
				OpenTag("tr");
				for ( var x = 0; x < state.Field.Width; x++ ) {
					OpenTag(
						"td",
						$"class=\"{GetFinalFieldClass(state, bestVariant, bestVariants, otherVariants, x, y)}\"");
					CloseTag();
				}
				CloseTag();
			}
			CloseTag();
		}

		public void WriteVariantState(IReadOnlyGameState state, IReadOnlyGameState variant) {
			OpenTag("table", "class=\"state-small\"");
			for ( var y = state.Field.Height; y >= 0; y-- ) {
				OpenTag("tr");
				for ( var x = 0; x < state.Field.Width; x++ ) {
					OpenTag(
						"td", $"class=\"{GetVariantFieldClass(state, variant, x, y)}\"");
					CloseTag();
				}
				CloseTag();
			}
			CloseTag();
		}

		public void WriteWithHeader(string header, string body) {
			OpenTag("p");
			Write("b", header + ": ");
			Write(body);
			CloseTag();
		}

		public void WriteWithHeader(string header, float value) =>
			WriteWithHeader(header, value.ToString("F4", CultureInfo.InvariantCulture));

		public void WriteWithHeader(string header, int value) =>
			WriteWithHeader(header, value.ToString());

		public void WriteVariant(string header, InputState[] inputs) {
			WriteWithHeader(header, ConvertInputsToString(inputs));
		}

		public void WriteVariants(string header, InputState[][] inputs) {
			WriteWithHeader(header, string.Join("; ", inputs.Select(ConvertInputsToString)));
		}

		string ConvertInputsToString(InputState[] inputs) {
			return "[" + string.Join(", ", inputs.Select(i => {
				switch ( i ) {
					case InputState.Rotate:    return "&uarr;";
					case InputState.MoveLeft:  return "&larr;";
					case InputState.MoveRight: return "&rarr;";
					default:                   return "-";
				}
			})) + "]";
		}

		string GetFinalFieldClass(
			IReadOnlyGameState state,
			IReadOnlyGameState bestVariant, IReadOnlyGameState[] bestVariants, IReadOnlyGameState[] otherVariants,
			int x, int y) {
			if ( state.Field.GetState(x, y) ) {
				return "field";
			}
			if ( IsFigurePresent(state, x, y) ) {
				return "figure";
			}
			if ( bestVariant.Field.GetState(x, y) ) {
				return "best";
			}
			if ( bestVariants.Any(s => s.Field.GetState(x, y)) ) {
				return "nice";
			}
			if ( otherVariants.Any(s => s.Field.GetState(x, y)) ) {
				return "bad";
			}
			return "empty";
		}

		string GetVariantFieldClass(IReadOnlyGameState state, IReadOnlyGameState variant, int x, int y) {
			if ( state.Field.GetState(x, y) ) {
				return "field";
			}
			if ( IsFigurePresent(state, x, y) ) {
				return "figure";
			}
			if ( variant.Field.GetState(x, y) ) {
				return "best";
			}
			return "empty";
		}

		bool IsFigurePresent(IReadOnlyGameState state, int x, int y) =>
			state.Figure.Elements
				.Any(e =>
					(Mathf.RoundToInt(state.Figure.Origin.x + e.x) == x) &&
					(Mathf.RoundToInt(state.Figure.Origin.y + e.y) == y));

		public void Dispose() {
			CloseTag();
			CloseTag();
			File.WriteAllText(_filePath, _content.ToString());
			Debug.Log($"Log saved to '{_filePath}'");
			Application.OpenURL($"file://{_filePath}");
		}
	}
}