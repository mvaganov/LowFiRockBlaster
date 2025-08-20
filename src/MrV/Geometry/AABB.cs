﻿namespace MrV.Geometry {
	public struct AABB {
		public Vec2 Min, Max;
		public float Width => (Max.x - Min.x);
		public float Height => (Max.y - Min.y);
		public AABB(AABB r) : this(r.Min, r.Max) { }
		public AABB(Vec2 min, Vec2 max) { Min = min; Max = max; }
		public AABB(float minx, float miny, float maxx, float maxy) :
			this(new Vec2(minx, miny), new Vec2(maxx, maxy)) { }
		public override string ToString() => $"[min{Min}, max{Max}, w/h({Width}, {Height})]";
	}
}
