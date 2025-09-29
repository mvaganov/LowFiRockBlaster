using MrV.CommandLine;
using MrV.Task;
using MrV.Geometry;
using System;
using MrV.GameEngine;
using System.Collections.Generic;

namespace MrV.LowFiRockBlaster {
	internal class Program {
		static void Main(string[] args) {
			int width = 80, height = 24;
			Vec2[] polygonShape = new Vec2[] { (25, 5), (35, 1), (50, 20) };
			bool running = true;
			Vec2 position = (18, 12);
			float radius = 10;
			float moveIncrement = 0.3f;
			float targetFps = 200;
			int targetMsDelay = (int)(1000 / targetFps);
			GraphicsContext graphics = new GraphicsContext(height, width);
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
			float particleSpeed = 5, particleRad = 2;
			ParticleSystem particles = new ParticleSystem((.25f, 1), (1, particleRad),
				(1, particleSpeed), 0.125f, ConsoleColor.White, FloatOverTime.GrowAndShrink);
			particles.Position = (10, 10);
			KeyInput.Bind(' ', () => particles.Emit(10), "explosion");
			float ScaleFactor = 1.25f;
			KeyInput.Bind('-', () => graphics.Scale *= ScaleFactor, "zoom out");
			KeyInput.Bind('=', () => graphics.Scale /= ScaleFactor, "zoom in");
			graphics.SetCameraCenter(particles.Position);

			List<Action<GraphicsContext>> drawPreProcessing = new List<Action<GraphicsContext>>();
			List<Action<GraphicsContext>> drawPostProcessing = new List<Action<GraphicsContext>>();
			void TestGraphics(GraphicsContext graphics) {
				graphics.DrawRectangle(0, 0, width, height, ConsoleGlyph.Default);
				graphics.DrawRectangle((2, 3), new Vec2(20, 15), ConsoleColor.Red);
				graphics.DrawRectangle(new AABB((10, 1), (15, 20)), ConsoleColor.Green);
				graphics.DrawCircle(position, radius, ConsoleColor.Blue);
				graphics.DrawPolygon(polygonShape, ConsoleColor.Yellow);
			}
			drawPreProcessing.Add(TestGraphics);
			drawPostProcessing.Add(particles.Draw);

			while (running) {
				Time.Update();
				Draw();
				Console.SetCursorPosition(0, height);
				Console.WriteLine($"{Time.DeltaTimeMs}   ");
				Input();
				Update();
				Time.SleepWithoutConsoleKeyPress(targetMsDelay);
			}

			void Draw() {
				Vec2 scale = (0.5f, 1);
				graphics.Clear();
				drawPreProcessing.ForEach(a => a.Invoke(graphics));
				// draw simulation elements here
				drawPostProcessing.ForEach(a => a.Invoke(graphics));
				graphics.PrintModifiedOnly();
				graphics.SwapBuffers();
				Console.SetCursorPosition(0, height);
			}
			void Input() {
				KeyInput.Read();
			}
			void Update() {
				KeyInput.TriggerEvents();
				Tasks.Update();
				particles.Update();
			}
		}
	}
}
