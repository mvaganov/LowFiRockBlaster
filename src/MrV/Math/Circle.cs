namespace MrV.Math {
	public struct Circle {
		public Vec2 center;
		public float radius;
		public Circle(Vec2 position, float radius) { this.center = position; this.radius = radius; }
		public override string ToString() => $"({center}, r:{radius})";
	}
}
