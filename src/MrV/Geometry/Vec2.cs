using System;

namespace MrV.Geometry {
	public struct Vec2 {
		public float x, y;
		public Vec2(float x, float y) { this.x = x; this.y = y; }
		public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.x + b.x, a.y + b.y);
		public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.x - b.x, a.y - b.y);
		public static implicit operator Vec2((float x, float y) tuple) => new Vec2(tuple.x, tuple.y);
		public override string ToString() => $"({x},{y})";
		public void Scale(Vec2 scale) { x *= scale.x; y *= scale.y; }
		public void InverseScale(Vec2 scale) { x /= scale.x; y /= scale.y; }
		public float MagnitudeSqr => x * x + y * y;
		public float Magnitude => MathF.Sqrt(MagnitudeSqr);
		public static Vec2 operator *(Vec2 vector, float scalar) => new Vec2(vector.x * scalar, vector.y * scalar);
		public static Vec2 operator /(Vec2 vector, float scalar) => new Vec2(vector.x / scalar, vector.y / scalar);
		public Vec2 Normalized => this / Magnitude;
		public Vec2 Perpendicular => new Vec2(y, -x);
	}
}
