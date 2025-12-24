namespace MrV.Geometry {
	public struct Circle {
		public Vec2 center;
		public float radius;
		public Circle(Vec2 position, float radius) { this.center = position; this.radius = radius; }
		public override string ToString() => $"({center}, r:{radius})";
		public static bool IsInsideCircle(Vec2 position, float radius, Vec2 point) {
			float dx = point.X - position.X, dy = point.Y - position.Y;
			return dx * dx + dy * dy <= radius * radius;
		}
		public bool Contains(Vec2 point) => IsInsideCircle(center, radius, point);
	}
}
