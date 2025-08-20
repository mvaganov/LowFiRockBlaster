using MrV.Geometry;

namespace MrV.CommandLine {
	public partial class DrawBuffer {
		public Vec2 ShapeScale = new Vec2(0.5f, 1);
		public delegate bool IsInsideShapeDelegate(Vec2 position);
		public void DrawShape(IsInsideShapeDelegate isInsideShape, Vec2 start, Vec2 end, char letterToPrint) {
			Vec2 renderStart = start;
			Vec2 renderEnd = end;
			renderStart.InverseScale(ShapeScale);
			renderEnd.InverseScale(ShapeScale);
			for (int y = (int)renderStart.y; y < renderEnd.y; ++y) {
				for (int x = (int)renderStart.x; x < renderEnd.x; ++x) {
					if (!IsValidLocation(y, x)) { continue; }
					bool pointIsInside = isInsideShape == null || isInsideShape(new Vec2(x * ShapeScale.x, y * ShapeScale.y));
					if (!pointIsInside) { continue; }
					WriteAt(letterToPrint, y, x);
				}
			}
		}
		public void DrawRectangle(Vec2 position, Vec2 size, char letterToPrint) {
			DrawShape(null, position, position+size, letterToPrint);
		}
		public void DrawRectangle(int x, int y, int width, int height, char letterToPrint) {
			DrawShape(null, new Vec2(x, y), new Vec2(x + width, y + height), letterToPrint);
		}
		public void DrawRectangle(AABB aabb, char letterToPrint) {
			DrawShape(null, aabb.Min, aabb.Max, letterToPrint);
		}
		public void DrawCircle(Circle c, char letterToPrint) {
			DrawCircle(c.center, c.radius, letterToPrint);
		}
		public void DrawCircle(Vec2 pos, float radius, char letterToPrint) {
			Vec2 extent = new Vec2(radius, radius);
			Vec2 start = pos - extent;
			Vec2 end = pos + extent;
			float r2 = radius * radius;
			DrawShape(IsInCircle, start, end, letterToPrint);
			bool IsInCircle(Vec2 point) {
				float dx = point.x - pos.x;
				float dy = point.y - pos.y;
				return dx * dx + dy * dy < r2;
			}
		}
		public void DrawPolygon(Vec2[] poly, char letterToPrint) {
			PolygonShape.TryGetAABB(poly, out Vec2 start, out Vec2 end);
			DrawShape(IsInPolygon, start, end, letterToPrint);
			bool IsInPolygon(Vec2 point) => PolygonShape.IsInPolygon(poly, point);
		}
	}
}
