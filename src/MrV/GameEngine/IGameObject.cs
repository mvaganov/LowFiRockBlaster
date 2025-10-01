using MrV.Geometry;

namespace MrV.GameEngine {
	public interface IGameObject : IDrawable {
		public string Name { get; set; }
		public Vec2 Position { get; set; }
		public bool IsActive { get; set; }
		public void Update();
	}
}
