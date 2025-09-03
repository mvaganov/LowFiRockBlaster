using System;

namespace MrV.CommandLine {
	public class GraphicsContext : DrawBuffer {
		private ConsoleGlyph[,] _lastBuffer;
		public GraphicsContext(int height, int width) : base(height, width) {}
		public override void SetSize(int height, int width) {
			base.SetSize(height, width);
			ResizeBuffer(ref _lastBuffer, height, width);
		}
		public virtual void PrintModifiedOnly() {
			for (int row = 0; row < Height; ++row) {
				bool mustMoveCursorToNewLocation = true;
				for (int col = 0; col < Width; ++col) {
					bool isSame = this[row, col].Equals(_lastBuffer[row, col]);
					if (isSame) {
						mustMoveCursorToNewLocation = true;
						continue;
					}
					ConsoleGlyph glyph = this[row, col];
					if (mustMoveCursorToNewLocation) {
						Console.SetCursorPosition(col, row);
						mustMoveCursorToNewLocation = false;
					}
					glyph.ApplyColor();
					glyph.Letter = '.';
					Console.Write(glyph);
				}
			}
			ConsoleGlyph.Default.ApplyColor();
		}
		public void SwapBuffers() {
			ConsoleGlyph[,] swap = _buffer;
			_buffer = _lastBuffer;
			_lastBuffer = swap;
		}
	}
}
