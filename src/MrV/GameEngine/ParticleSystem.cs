using MrV.CommandLine;
using MrV.Geometry;
using System;

namespace MrV.GameEngine {
	internal class ParticleSystem {
		public PolicyDrivenObjectPool<Particle> ParticlePool = new PolicyDrivenObjectPool<Particle>();
		public FloatOverTime SizeOverLifetime;
		public ConsoleColor Color = ConsoleColor.White;
		public Vec2 Position;
		public RangeF ParticleLifetime;
		public RangeF ParticleSize;
		public RangeF ParticleSpeed;
		public RangeF SpawnRadius;

		public ParticleSystem(RangeF particleLifetime, RangeF particleSize, RangeF particleSpeed,
		RangeF spawnRadius, ConsoleColor color, FloatOverTime sizeOverLifetime) {
			ParticlePool.Setup(CreateParticle, CommissionParticle, DecommissionParticle);
			ParticleLifetime = particleLifetime;
			ParticleSize = particleSize;
			ParticleSpeed = particleSpeed;
			SpawnRadius = spawnRadius;
			Color = color;
			SizeOverLifetime = sizeOverLifetime;
		}
		public Particle CreateParticle() => new Particle(new Circle(Position,0), default, Color, 0);
		public void CommissionParticle(Particle particle) {
			particle.Enabled = true;
			particle.LifetimeCurrent = 0;
			particle.OriginalSize = ParticleSize.Random;
			particle.Circle.radius = particle.OriginalSize * GetSizeAtTime(0);
			particle.Velocity = default;
			particle.Color = Color;
			particle.LifetimeMax = ParticleLifetime.Random;
			Vec2 direction = Vec2.ConvertDegrees(360 * Rand.Number);
			particle.Velocity = direction * ParticleSpeed.Random;
			particle.Circle.center = Position + direction * SpawnRadius.Random;
		}
		public void DecommissionParticle(Particle particle) {
			particle.Enabled = false;
		}
		public float GetSizeAtTime(float lifetimePercentage) {
			if (SizeOverLifetime == null
			|| !SizeOverLifetime.TryGetValue(lifetimePercentage, out float value)) { return 1; }
			return value;
		}
		public void Draw(GraphicsContext graphics) {
			for (int i = 0; i < ParticlePool.Count; ++i) {
				ParticlePool[i].Draw(graphics);
			}
		}
		public void Update() {
			for (int i = 0; i < ParticlePool.Count; ++i) {
				// possible to update decommissioned object. cost of updating stale object assumed less than servicing decommissions every iteration.
				ParticlePool[i].Update();
				float timeProgress = ParticlePool[i].LifetimeCurrent / ParticlePool[i].LifetimeMax;
				ParticlePool[i].Circle.radius = GetSizeAtTime(timeProgress) * ParticlePool[i].OriginalSize;
				if (timeProgress >= 1) {
					ParticlePool.DecommissionDelayedAtIndex(i);
				}
			}
			ParticlePool.ServiceDelayedDecommission();
		}
		public void Emit(int count = 1) {
			for (int i = 0; i < count; ++i) {
				ParticlePool.Commission();
			}
		}
	}
}
