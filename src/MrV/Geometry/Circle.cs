namespace MrV.Geometry {
	public struct Circle {
		public Vec2 Center;
		public float Radius;
		public Circle(Vec2 position, float radius) { Center = position; Radius = radius; }
		public override string ToString() => $"({Center}, r:{Radius})";
		public static bool IsInsideCircle(Vec2 position, float radius, Vec2 point) {
			float dx = point.X - position.X, dy = point.Y - position.Y;
			return dx * dx + dy * dy <= radius * radius;
		}
		public bool Contains(Vec2 point) => IsInsideCircle(Center, Radius, point);
	}
}
