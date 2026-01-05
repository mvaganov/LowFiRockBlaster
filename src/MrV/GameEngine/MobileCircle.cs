using MrV.CommandLine;
using MrV.Geometry;

namespace MrV.GameEngine {
	public class MobileCircle : MobileObject {
		public Circle Circle;
		public override Vec2 Position { get => Circle.Center; set => Circle.Center = value; }
		public override Vec2 Direction { get => Velocity.Equals(Vec2.Zero) ? Vec2.ZeroDegrees : Velocity.Normalized(); set { } }
		public MobileCircle(Circle circle) { Circle = circle; }
		public override void Draw(GraphicsContext canvas) {
			if (!_active) { return; }
			canvas.SetColor(Color);
			canvas.DrawCircle(Circle);
		}
	}
}
