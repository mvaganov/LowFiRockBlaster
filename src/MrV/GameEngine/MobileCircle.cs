using MrV.CommandLine;
using MrV.Geometry;

namespace MrV.GameEngine {
	public class MobileCircle : MobileObject {
		private Circle Circle;
		public override Vec2 Position { get => Circle.Center; set => Circle.Center = value; }
		public override Vec2 Direction { get => Velocity.Normalized(); set { } }
		public float Radius { get => Circle.Radius; set => Circle.Radius = value; }
		public static bool DebugShowVelocity = false;
		public MobileCircle(Circle circle) {
			Circle = circle;
		}
		public virtual void Copy(MobileCircle other) {
			base.Copy(other);
			Circle = other.Circle;
		}
		public Circle GetCollisionBoundingCircle() => Circle;
		public override void Draw(GraphicsContext canvas) {
			if (!_active) return;
			canvas.SetColor(Color);
			canvas.DrawCircle(Circle);
			if (DebugShowVelocity) {
				ShowDebugVelocity(canvas);
			}
		}

		private void ShowDebugVelocity(GraphicsContext graphicsContext) {
			float speed = Velocity.Length();
			if (speed == 0) { return; }
			Vec2 dir = Velocity / speed;
			Vec2 start = Position + dir * Radius;
			Vec2 end = start + Velocity;
			graphicsContext.SetColor(Color);
			graphicsContext.DrawLine(start, end, 1);
		}
	}
}
