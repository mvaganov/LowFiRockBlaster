using System;

namespace MrV.Geometry {
	public struct Vec2 {
		public float X, Y;
		public Vec2(float x, float y) { X = x; Y = y; }
		public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.X + b.X, a.Y + b.Y);
		public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.X - b.X, a.Y - b.Y);
		public static Vec2 operator *(Vec2 a, Vec2 b) => new Vec2(a.X * b.X, a.Y * b.Y);
		public static Vec2 operator /(Vec2 a, Vec2 b) => new Vec2(a.X / b.X, a.Y / b.Y);
		public static implicit operator Vec2((float x, float y) tuple) => new Vec2(tuple.x, tuple.y);
		public override string ToString() => $"({X},{Y})";
		public float LengthSquared() => X * X + Y * Y;
		public float Length() => MathF.Sqrt(LengthSquared());
		public static Vec2 operator *(Vec2 vector, float scalar) => new Vec2(vector.X * scalar, vector.Y * scalar);
		public static Vec2 operator /(Vec2 vector, float scalar) => new Vec2(vector.X / scalar, vector.Y / scalar);
		public Vec2 Normalized() => this / Length();
		public Vec2 Perpendicular() => new Vec2(Y, -X);
		public static float DegreesToRadians(float degrees) => degrees * MathF.PI / 180;
		public static float RadiansToDegrees(float radians) => radians * 180 / MathF.PI;
		public static Vec2 ConvertRadians(float radians) => new Vec2(MathF.Cos(radians), MathF.Sin(radians));
		public static Vec2 ConvertDegrees(float degrees) => ConvertRadians(DegreesToRadians(degrees));
		public float NormalToRadians() => MathF.Atan2(Y, X);
		public float NormalToDegrees() => RadiansToDegrees(NormalToRadians());
		public static float WrapRadian(float radian) => MathF.IEEERemainder(radian, 2 * MathF.PI);
		public static float WrapDegrees(float degrees) => MathF.IEEERemainder(degrees, 360);
	}
}
