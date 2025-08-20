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
				bool edgeVerticallySpansRay = (vertex.y > pt.y) != (prevVertex.y > pt.y);
				if (!edgeVerticallySpansRay) {
					continue;
				}
				float slope = (prevVertex.x - vertex.x) / (prevVertex.y - vertex.y);
				float xIntersection = slope * (pt.y - vertex.y) + vertex.x;
				bool intersect = pt.x < xIntersection;
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
				if (p.x < min.x) { min.x = p.x; }
				if (p.y < min.y) { min.y = p.y; }
				if (p.x > max.x) { max.x = p.x; }
				if (p.y > max.y) { max.y = p.y; }
			}
			return true;
		}
	}
}
