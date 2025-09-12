using MrV.CommandLine;
using MrV.Task;
using MrV.Geometry;
using System;
using MrV.GameEngine;

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
			ObjectPool<Particle> particlesPool = new ObjectPool<Particle>();
			float particleSpeed = 5, particleRad = 2;
			particlesPool.Setup(
				() => new Particle(new Circle(default, 1), default, ConsoleColor.White, 1),
				p => {
					Vec2 direction = Vec2.ConvertDegrees(Rand.Number * 360);
					p.Init(new Circle((10, 10), Rand.Number * particleRad), direction * Rand.Number * particleSpeed,
						ConsoleColor.White, Rand.Range(.25f, 1));
				},
				p => p.Enabled = false);
			Rand.Instance.Seed = (uint)Time.CurrentTimeMs;
			KeyInput.Bind(' ', () => {
				for (int i = 0; i < 10; ++i) {
					particlesPool.Commission();
				}
			}, "explosion");
			FloatOverTime growAndShrink = FloatOverTime.GrowAndShrink;
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
				graphics.DrawRectangle(0, 0, width, height, ConsoleGlyph.Default);
				graphics.DrawRectangle((2, 3), new Vec2(20, 15), ConsoleColor.Red);
				graphics.DrawRectangle(new AABB((10, 1), (15, 20)), ConsoleColor.Green);
				graphics.DrawCircle(position, radius, ConsoleColor.Blue);
				graphics.DrawPolygon(polygonShape, ConsoleColor.Yellow);
				for (int i = 0; i < particlesPool.Count; ++i) {
					particlesPool[i].Draw(graphics);
				}
				graphics.PrintModifiedOnly();
				graphics.SwapBuffers();
				Console.SetCursorPosition(0, (int)height);
			}
			void Input() {
				KeyInput.Read();
			}
			void Update() {
				KeyInput.TriggerEvents();
				Tasks.Update();
				for (int i = 0; i < particlesPool.Count; ++i) {
					particlesPool[i].Update();
					float timeProgress = particlesPool[i].LifetimeCurrent / particlesPool[i].LifetimeMax;
					if (growAndShrink.TryGetValue(timeProgress, out float nextRadiusPercentage)) {
						particlesPool[i].Circle.radius = nextRadiusPercentage * particlesPool[i].OriginalSize;
					}
					if (timeProgress >= 1) {
						particlesPool.DecommissionDelayedAtIndex(i);
					}
				}
				particlesPool.ServiceDelayedDecommission();
			}
		}
	}
}
