using MrV.CommandLine;

namespace MrV.GameEngine {
	public interface IDrawable {
		public bool IsVisible { get; set; }
		public void Draw(GraphicsContext canvas);
	}
}
