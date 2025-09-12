using MrV.CommandLine;
using MrV.Geometry;
using System;

namespace MrV.GameEngine {
	public class Particle {
		public Circle Circle;
		public ConsoleColor Color;
		public Vec2 Velocity;
		public bool enabled;
		public float lifetimeMax, lifetimeCurrent;
		public Particle(Circle circle, Vec2 velocity, ConsoleColor color, float lifetime) {
			Init(circle, velocity, color, lifetime);
		}
		public void Init(Circle circle, Vec2 velocity, ConsoleColor color, float lifetime) {
			Circle = circle;
			Velocity = velocity;
			Color = color;
			enabled = true;
			lifetimeMax = lifetime;
			lifetimeCurrent = 0;
		}
		public void Draw(GraphicsContext g) {
			if (!enabled) { return; }
			g.DrawCircle(Circle, Color);
			//float speed = Velocity.Magnitude;
			//if (speed > 0) {
			//	Vec2 direction = Velocity / speed;
			//	Vec2 rayStart = Circle.center + direction * Circle.radius;
			//	g.DrawLine(rayStart, rayStart + Velocity, 0.5f, Color);
			//}
		}
		public void Update() {
			if (!enabled) { return; }
			lifetimeCurrent += Time.DeltaTimeSec;
			if (lifetimeCurrent >= lifetimeMax) {
				enabled = false;
				return;
			}
			Vec2 moveThisFrame = Velocity * Time.DeltaTimeSec;
			Circle.center += moveThisFrame;
		}
	}
}
