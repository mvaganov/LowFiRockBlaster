using MrV.CommandLine;
using MrV.Geometry;
using System;

namespace MrV.GameEngine {
	public class Particle {
		public Circle Circle;
		public ConsoleColor Color;
		public Vec2 Velocity;
		public Particle(Circle circle, Vec2 velocity, ConsoleColor color) {
			Circle = circle;
			Velocity = velocity;
			Color = color;
		}
		public void Draw(GraphicsContext g) {
			g.DrawCircle(Circle, Color);
			float speed = Velocity.Magnitude;
			if (speed > 0) {
				Vec2 direction = Velocity / speed;
				Vec2 rayStart = Circle.center + direction * Circle.radius;
				g.DrawLine(rayStart, rayStart + Velocity, 0.5f, Color);
			}
		}
		public void Update() {
			Vec2 moveThisFrame = Velocity * Time.DeltaTimeSec;
			Circle.center += moveThisFrame;
		}
	}
}
