using System.Collections.Generic;

namespace MrV.Geometry {
	public struct PolygonShape {
		public Vec2[] points;
		public PolygonShape(Vec2[] points) {
			this.points = points;
		}
		public override string ToString() => $"(polygon: {string.Join(", ", points)})";
		public static bool IsInPolygon(IList<Vec2> poly, Vec2 pt) {
			bool inside = false;
			for (int index = 0, prevIndex = poly.Count - 1; index < poly.Count; prevIndex = index++) {
				Vec2 vertex = poly[index], prevVertex = poly[prevIndex];
				bool edgeVerticallySpansRay = (vertex.Y > pt.Y) != (prevVertex.Y > pt.Y);
				if (!edgeVerticallySpansRay) {
					continue;
				}
				float slope = (prevVertex.X - vertex.X) / (prevVertex.Y - vertex.Y);
				float xIntersection = slope * (pt.Y - vertex.Y) + vertex.X;
				bool intersect = pt.X < xIntersection;
				if (intersect) {
					inside = !inside;
				}
			}
			return inside;
		}
		public static bool TryGetAABB(IList<Vec2> points, out Vec2 min, out Vec2 max) {
			if (points.Count == 0) {
				min = max = default;
				return false;
			}
			min = max = points[0];
			for (int i = 1; i < points.Count; ++i) {
				Vec2 p = points[i];
				if (p.X < min.X) { min.X = p.X; }
				if (p.Y < min.Y) { min.Y = p.Y; }
				if (p.X > max.X) { max.X = p.X; }
				if (p.Y > max.Y) { max.Y = p.Y; }
			}
			return true;
		}
	}
}
