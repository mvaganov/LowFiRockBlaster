using MrV.Math;
using System;

namespace MrV.LowFiRockBlaster {
	internal class Program {
		public static void DrawRectangle(Vec2 position, Vec2 size, char letterToPrint) {
			DrawRectangle((int)position.x, (int)position.y, (int)size.x, (int)size.y, letterToPrint);
		}
		public static void DrawRectangle(int x, int y, int width, int height, char letterToPrint) {
			for (int row = 0; row < height; ++row) {
				for (int col = 0; col < width; ++col) {
					Console.SetCursorPosition(col + x, row + y);
					Console.Write(letterToPrint);
				}
			}
		}
		public static void DrawRectangle(int width, int height, char letterToPrint) {
			for (int row = 0; row < height; ++row) {
				for (int col = 0; col < width; ++col) {
					Console.Write(letterToPrint);
				}
				Console.WriteLine();
			}
		}
		static void Main(string[] args) {
			int width = 80, height = 25;
			char letterToPrint = '#';
			DrawRectangle(0, 0, width, height, letterToPrint);
		}
	}
}
