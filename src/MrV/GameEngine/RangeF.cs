namespace MrV.GameEngine {
	public class RangeF {
		public float Min, Max;
		public float Delta => Max - Min;
		public float Random => Min + Delta * Rand.Number;
		public RangeF(float min, float max) { Min = min; Max = max; }
		public static implicit operator RangeF((float min, float max) tuple) => new RangeF(tuple.min, tuple.max);
		public static implicit operator RangeF(float singleValue) => new RangeF(singleValue, singleValue);
	}
}
