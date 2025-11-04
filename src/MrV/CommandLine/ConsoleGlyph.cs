using System;
using System.Text;

namespace MrV.CommandLine {
	public struct ConsoleGlyph {
		private char letter;
		private ConsoleColorPair colorPair;
		public char Letter {
			get { return letter; }
			set {
				letter = value;
				if (letter != '\n' && (letter < 32 || letter > 126)) {
					throw new Exception("out of ascii range");
				}
			}
		}
		public ConsoleColor Fore { get { return colorPair.fore; } set { colorPair.fore = value; } }
		public ConsoleColor Back { get { return colorPair.back; } set { colorPair.back = value; } }
		public ConsoleGlyph(char letter, ConsoleColorPair colorPair) { this.letter = letter; this.colorPair = colorPair; }
		public static implicit operator ConsoleGlyph(ConsoleColor color) => new ConsoleGlyph(' ', Default.Fore, color);
		public ConsoleGlyph(char letter, ConsoleColor fore, ConsoleColor back) :
			this(' ', new ConsoleColorPair(fore, back)) { }
		public static readonly ConsoleGlyph Default = new ConsoleGlyph(' ', ConsoleColorPair.Default);
		public static readonly ConsoleGlyph Empty = new ConsoleGlyph('\0', ConsoleColor.Black, ConsoleColor.Black);
		public bool Equals(ConsoleGlyph other) => other.letter == letter && other.Fore == Fore && other.Back == Back;
		public override string ToString() => letter.ToString();
		public void ApplyColor() => colorPair.Apply();
		public static ConsoleGlyph[] Convert(string text,
			ConsoleColor fore = ConsoleColor.Gray, ConsoleColor back = ConsoleColor.Black) {
			ConsoleGlyph[] result = new ConsoleGlyph[text.Length];
			for (int i = 0; i < result.Length; i++) {
				ConsoleGlyph g = new ConsoleGlyph(text[i], fore, back);
				result[i] = g;
			}
			return result;
		}
		public static string Convert(ConsoleGlyph[] text) {
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < text.Length; ++i) {
				sb.Append(text[i].letter);
			}
			return sb.ToString();
		}
	}
}
