using MrV.CommandLine;
using MrV.Geometry;
using System;

namespace MrV.GameEngine {
	public abstract class MobileObject : IGameObject {
		protected string _name;
		protected bool _active = true;
		protected Vec2 _velocity;
		protected ConsoleColor _color;
		public virtual string Name { get => _name; set => _name = value; }
		public virtual Vec2 Velocity { get => _velocity; set => _velocity = value; }
		public virtual bool IsActive { get => _active; set => _active = value; }
		public virtual bool IsVisible { get => IsActive; set => IsActive = value; }
		public abstract Vec2 Position { get; set; }
		public abstract Vec2 Direction { get; set; }
		public virtual float RotationRadians {
			get => Direction.NormalToRadians();
			set { Direction = Vec2.ConvertRadians(value); }
		}
		public virtual float RotationDegrees {
			get => Direction.NormalToDegrees();
			set { Direction = Vec2.ConvertDegrees(value); }
		}
		public ConsoleColor Color { get => _color; set => _color = value; }
		public byte TypeId { get; set; }
		public abstract void Draw(GraphicsContext canvas);
		public virtual void Update() {
			if (!_active) { return; }
			Vec2 moveThisFrame = _velocity * Time.DeltaTimeSec;
			Position += moveThisFrame;
		}
		public virtual void Copy(MobileObject other) {
			TypeId = other.TypeId;
			IsActive = other._active;
			Velocity = other._velocity;
			Color = other._color;
		}
	}
}
