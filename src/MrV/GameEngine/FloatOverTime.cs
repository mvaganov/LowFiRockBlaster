using System.Collections.Generic;

namespace MrV.GameEngine {
	public class FloatOverTime : ValueOverTime<float> {
		public static FloatOverTime GrowAndShrink = new FloatOverTime(new Frame<float>[] {
			new Frame<float>(0, 0), new Frame<float>(0.5f, 1), new Frame<float>(1, 0)
		});
		public FloatOverTime(IList<Frame<float>> curve) : base(curve) {}
		public override float Lerp(float percentageProgress, float start, float end) {
			float delta = end - start;
			return start + delta * percentageProgress;
		}
	}
	public abstract class ValueOverTime<T> {
		public struct Frame<T> {
			public float time;
			public T value;
			public Frame(float time, T value) {
				this.time = time;
				this.value = value;
			}
		}
		public bool WrapTime = false;
		public IList<Frame<T>> curve;
		public float StartTime => curve[0].time;
		public float EndTime => curve[curve.Count - 1].time;
		public abstract T Lerp(float t, T start, T end);
		public ValueOverTime(IList<Frame<T>> curve) {
			this.curve = curve;
		}
		public bool TryGetValue(float time, out T value) {
			if (curve == null || curve.Count == 0) {
				value = default;
				return false;
			}
			int index = Algorithm.BinarySearch(curve, time, frame => frame.time);
			if (index >= 0) {
				value = curve[index].value;
				return true;
			}
			index = ~index;
			if (index == 0) {
				value = curve[0].value;
				return true;
			}
			if (index >= curve.Count) {
				value = curve[curve.Count - 1].value;
				return true;
			}
			Frame<T> prev = curve[index - 1];
			Frame<T> next = curve[index];
			float normalizedTimeProgress = CalcProgressBetweenStartAndEnd(time, prev.time, next.time);
			value = Lerp(normalizedTimeProgress, prev.value, next.value);
			return true;
		}
		public static float CalcProgressBetweenStartAndEnd(float t, float start, float end) {
			float timeDelta = end - start;
			float timeProgress = t - start;
			return timeProgress / timeDelta;
		}
	}
}
