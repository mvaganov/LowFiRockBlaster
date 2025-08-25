using MrV.CommandLine;
using MrV.Task;
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
			KeyInput.Bind('w', () => position.y -= moveIncrement, "move circle up");
			KeyInput.Bind('a', () => position.x -= moveIncrement, "move circle left");
			KeyInput.Bind('s', () => position.y += moveIncrement, "move circle down");
			KeyInput.Bind('d', () => position.x += moveIncrement, "move circle right");
			KeyInput.Bind('e', () => radius += moveIncrement, "expand radius");
			KeyInput.Bind('r', () => radius -= moveIncrement, "reduce radius");
			KeyInput.Bind((char)27, () => running = false, "quit");
			int timeMs = 0;
			int keyDelayMs = 20;
			for (int i = 0; i < 10; ++i) {
				Tasks.Add(() => KeyInput.Add('d'), timeMs);
				timeMs += keyDelayMs;
				Tasks.Add(() => KeyInput.Add('e'), timeMs);
				timeMs += keyDelayMs;
			}
			for (int i = 0; i < 20; ++i) {
				Tasks.Add(() => KeyInput.Add('w'), timeMs);
				timeMs += keyDelayMs;
				Tasks.Add(() => KeyInput.Add('r'), timeMs);
				timeMs += keyDelayMs;
			}
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
				KeyInput.Read();
			}
			void Update() {
				KeyInput.TriggerEvents();
				Tasks.Update();
			}
		}
	}
}
