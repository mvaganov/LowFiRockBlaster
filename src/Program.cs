using MrV.CommandLine;
using MrV.Geometry;
using System;

namespace MrV.LowFiRockBlaster {
	internal class Program {
		static void Main(string[] args) {
			int width = 80, height = 24;
			char letterToPrint = '#';
			Vec2[] polygonShape = new Vec2[] { (25, 5), (35, 1), (50, 20) };
			bool running = true;
			Vec2 position = (18, 12);
			float radius = 10;
			float moveIncrement = 0.3f;
			char input = (char)0;
			float targetFps = 200;
			int targetMsDelay = (int)(1000 / targetFps);
			DrawBuffer graphics = new DrawBuffer(height, width);
			while (running) {
				Time.Update();
				Draw();
				Console.SetCursorPosition(0, (int)height);
				Console.WriteLine($"{Time.DeltaTimeMs}   ");
				Input();
				Update();
				Time.SleepWithoutConsoleKeyPress(targetMsDelay);
			}

			void Draw() {
				Vec2 scale = (0.5f, 1);
				graphics.Clear();
				graphics.DrawRectangle(0, 0, width, height, letterToPrint);
				graphics.DrawRectangle((2, 3), new Vec2(20, 15), '*');
				graphics.DrawRectangle(new AABB((10, 1), (15, 20)), '|');
				graphics.DrawCircle(position, radius, '.');
				graphics.DrawPolygon(polygonShape, '-');
				graphics.Print();
				Console.SetCursorPosition(0, (int)height);
			}
			void Input() {
				if (Console.KeyAvailable) {
					while (Console.KeyAvailable) {
						input = Console.ReadKey().KeyChar;
					}
				} else {
					input = (char)0;
				}
			}
			void Update() {
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
