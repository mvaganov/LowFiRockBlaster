using System;

namespace MrV.CommandLine {
	public class GraphicsContext : DrawBuffer {
		private char[,] _lastBuffer;
		public GraphicsContext(int width, int height) : base(width, height) {}
		public override void SetSize(int width, int height) {
			base.SetSize(width, height);
			ResizeBuffer(ref _lastBuffer, height, width);
		}
		public virtual void PrintModifiedCharactersOnly() {
			for (int row = 0; row < Height; ++row) {
				for (int col = 0; col < Width; ++col) {
					bool isSame = this[row, col] == _lastBuffer[row, col];
					if (isSame) {
						continue;
					}
					char glyph = this[row, col];
					Console.SetCursorPosition(col, row);
					Console.Write(glyph);
				}
			}
		}
		public void FinishedRender() {
			SwapBuffers();
			Clear();
		}
		public void SwapBuffers() {
			char[,] swap = _buffer;
			_buffer = _lastBuffer;
			_lastBuffer = swap;
		}
	}
}
