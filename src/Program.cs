using MrV.Math;
using System;

namespace MrV.LowFiRockBlaster {
	internal class Program {
		public static void DrawRectangle(Vec2 position, Vec2 size, char letterToPrint) {
			DrawRectangle((int)position.x, (int)position.y, (int)size.x, (int)size.y, letterToPrint);
		}
		public static void DrawRectangle(int x, int y, int width, int height, char letterToPrint) {
			for (int row = 0; row < height; ++row) {
				for (int col = 0; col < width; ++col) {
					Console.SetCursorPosition(col + x, row + y);
					Console.Write(letterToPrint);
				}
			}
		}
		public static void DrawRectangle(int width, int height, char letterToPrint) {
			for (int row = 0; row < height; ++row) {
				for (int col = 0; col < width; ++col) {
					Console.Write(letterToPrint);
				}
				Console.WriteLine();
			}
		}
		public static void DrawRectangle(AABB aabb, char letterToPrint) {
			DrawRectangle((int)aabb.Min.x, (int)aabb.Min.y, (int)aabb.Width, (int)aabb.Height, letterToPrint);
		}
		public static void DrawCircle(Circle c, char letterToPrint) {
			DrawCircle(c.center, c.radius, letterToPrint);
		}
		public static void DrawCircle(Vec2 pos, float radius, char letterToPrint) {
			Vec2 extent = (radius, radius); // Vec2 knows how to convert from a tuple of floats
			Vec2 start = pos - extent;
			Vec2 end = pos + extent;
			Vec2 coord = start;
			float r2 = radius * radius;
			for (; coord.y < end.y; coord.y += 1) {
				coord.x = start.x;
				for (; coord.x < end.x; coord.x += 1) {
					if (coord.x < 0 || coord.y < 0) { continue; }
					Vec2 d = coord - pos;
					bool pointIsInside = d.x * d.x + d.y * d.y < r2;
					if (pointIsInside) {
						Console.SetCursorPosition((int)coord.x, (int)coord.y);
						Console.Write(letterToPrint);
					}
				}
			}
		}
		public static void DrawPolygon(Vec2[] poly, char letterToPrint) {
			PolygonShape.TryGetAABB(poly, out Vec2 start, out Vec2 end);
			Vec2 coord = start;
			for (; coord.y < end.y; coord.y += 1) {
				coord.x = start.x;
				for (; coord.x < end.x; coord.x += 1) {
					if (coord.x < 0 || coord.y < 0) { continue; }
					bool pointIsInside = PolygonShape.IsInPolygon(poly, coord);
					if (pointIsInside) {
						Console.SetCursorPosition((int)coord.x, (int)coord.y);
						Console.Write(letterToPrint);
					}
				}
			}
		}
		static void Main(string[] args) {
			int width = 80, height = 24;
			char letterToPrint = '#';
			Vec2[] polygonShape = new Vec2[] { (25, 5), (35, 1), (50, 20) };
			bool running = true;
			Vec2 position = (18, 12);
			float radius = 10;
			float moveIncrement = 0.5f;
			while (running) {
				DrawRectangle(0, 0, width, height, letterToPrint);
				DrawRectangle((2, 3), new Vec2(20, 15), '*');
				DrawRectangle(new AABB((10, 1), (15, 20)), '|');
				DrawCircle(position, radius, '.');
				DrawPolygon(polygonShape, '-');
				Console.SetCursorPosition(0, (int)height);
				char input = Console.ReadKey().KeyChar;
				switch (input) {
					case 'w': position.y -= moveIncrement; break;
					case 'a': position.x -= moveIncrement; break;
					case 's': position.y += moveIncrement; break;
					case 'd': position.x += moveIncrement; break;
					case 'e': radius += moveIncrement; break;
					case 'r': radius -= moveIncrement; break;
					case (char)27: running = false; break;
				}
			}
		}
	}
}
