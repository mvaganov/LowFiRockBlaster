using System;

namespace MrV.CommandLine {
	public struct ConsoleColorPair {
		private byte _fore, _back;
		public ConsoleColor fore { get => (ConsoleColor)_fore; set => _fore = (byte)value; }
		public ConsoleColor back { get => (ConsoleColor)_back; set => _back = (byte)value; }
		public ConsoleColorPair(ConsoleColor fore, ConsoleColor back) {
			_back = (byte)back;
			_fore = (byte)fore;
		}
		public void Apply() {
			Console.ForegroundColor = fore;
			Console.BackgroundColor = back;
		}
		public static ConsoleColorPair Default = new ConsoleColorPair(ConsoleColor.Gray, ConsoleColor.Black);
		public static ConsoleColorPair Current => new ConsoleColorPair(Console.ForegroundColor, Console.BackgroundColor);
		static ConsoleColorPair() {
			Default = Current;
		}
	}
}
