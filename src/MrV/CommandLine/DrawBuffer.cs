using MrV.Geometry;
using System;

namespace MrV.CommandLine {
	public partial class DrawBuffer {
		protected ConsoleGlyph[,] _buffer;
		public int Height => GetHeight(_buffer);
		public static int GetHeight(ConsoleGlyph[,] buffer) => buffer.GetLength(0);
		public int Width => GetWidth(_buffer);
		public static int GetWidth(ConsoleGlyph[,] buffer) => buffer.GetLength(1);
		public Vec2 Size => new Vec2(Width, Height);
		public ConsoleGlyph this[int y, int x] {
			get => _buffer[y, x];
			set => _buffer[y, x] = value;
		}
		public DrawBuffer(int height, int width) {
			SetSize(height, width);
		}
		public virtual void SetSize(int height, int width) {
			ResizeBuffer(ref _buffer, height, width);
		}
		protected static void ResizeBuffer(ref ConsoleGlyph[,] buffer, int height, int width) {
			ConsoleGlyph[,] oldBuffer = buffer;
			buffer = new ConsoleGlyph[height, width];
			if (oldBuffer == null) {
				return;
			}
			int oldH = GetHeight(oldBuffer), oldW = GetWidth(oldBuffer);
			int rowsToCopy = Math.Min(height, oldH), colsToCopy = Math.Min(width, oldW);
			for (int y = 0; y < rowsToCopy; ++y) {
				for (int x = 0; x < colsToCopy; ++x) {
					buffer[y, x] = oldBuffer[y, x];
				}
			}
		}
		public Vec2 WriteAt(string text, int row, int col) => WriteAt(ConsoleGlyph.Convert(text), row, col);
		public Vec2 WriteAt(ConsoleGlyph[] text, int row, int col) {
			for (int i = 0; i < text.Length; i++) {
				ConsoleGlyph glyph = text[i];
				switch (glyph.Letter) {
					case '\n': ++row; col = 0; break;
					default: WriteAt(glyph, row, col++); break;
				}
			}
			return new Vec2(col, row);
		}
		public void WriteAt(ConsoleGlyph glyph, int row, int col) {
			if (!IsValidLocation(row, col)) {
				return;
			}
			_buffer[row, col] = glyph;
		}
		public bool IsValidLocation(int y, int x) => x >= 0 && x < Width && y >= 0 && y < Height;
		public void Clear() => Clear(_buffer, ConsoleGlyph.Default);
		public static void Clear(ConsoleGlyph[,] buffer, ConsoleGlyph background) {
			for (int row = 0; row < GetHeight(buffer); ++row) {
				for (int col = 0; col < GetWidth(buffer); ++col) {
					buffer[row, col] = background;
				}
			}
		}
		public virtual void Print() => PrintBuffer(_buffer);
		public static void PrintBuffer(ConsoleGlyph[,] buffer) {
			int height = GetHeight(buffer), width = GetWidth(buffer);
			for (int row = 0; row < height; ++row) {
				Console.SetCursorPosition(0, row);
				for (int col = 0; col < width; ++col) {
					ConsoleGlyph glyph = buffer[row, col];
					glyph.ApplyColor();
					Console.Write(glyph);
				}
			}
			ConsoleGlyph.Default.ApplyColor();
		}
	}
}
