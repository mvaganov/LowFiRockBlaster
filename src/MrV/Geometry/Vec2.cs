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
		public void Floor() { x = MathF.Floor(x); y = MathF.Floor(y); }
		public void Ceil() { x = MathF.Ceiling(x); y = MathF.Ceiling(y); }
		public static Vec2 One = new Vec2(1, 1);
	}
}
