using System;
using System.Diagnostics;

namespace MrV {
	/// <summary>
	/// Keeps track of timing, specifically for frame-based update in a game loop.
	/// <list type="bullet">
	/// <item>Uses C# <see cref="Stopwatch"/> as cannonical timer implementation</item>
	/// <item>Floating point values are convenient for physics calculations</item>
	/// <item>Floating point timestamps are stored as 'double' for precision, since 'float' becomes less accurate than 1ms after 4.5 hours</item>
	/// <item>Time is also calculated in milliseconds, since all floating points (even doubles) become less accurate as values increase</item>
	/// </list>
	/// </summary>
	public partial class Time {
		protected Stopwatch _timer;
		protected long _deltaTimeMs;
		protected float _deltaTimeSec;
		protected long _timeMsOfCurrentFrame;
		protected double _timeSecOfCurrentFrame;
		protected static Time _instance;
		public static long CurrentTimeMs => DateTimeOffset.Now.ToUnixTimeMilliseconds();
		public long DeltaTimeMilliseconds => _deltaTimeMs;
		public float DeltaTimeSeconds => _deltaTimeSec;
		public long TimeMillisecondsCurrentFrame => _timeMsOfCurrentFrame;
		public double TimeSecondsCurrentFrame => _timeSecOfCurrentFrame;
		public long DeltaTimeMsCalculateNow => _timer.ElapsedMilliseconds - _timeMsOfCurrentFrame;
		public float DeltaTimeSecCalculateNow => (float)(_timer.Elapsed.TotalSeconds - _timeSecOfCurrentFrame);
		public static long TimeMsCurrentFrame => Instance.TimeMillisecondsCurrentFrame;
		public static double TimeSecCurrentFrame => Instance.TimeSecondsCurrentFrame;
		public static Time Instance => _instance != null ? _instance : _instance = new Time();
		public static long DeltaTimeMs => Instance.DeltaTimeMilliseconds;
		public static float DeltaTimeSec => Instance.DeltaTimeSeconds;
		public static void Update() => Instance.UpdateTiming();
		public static void SleepWithoutConsoleKeyPress(int ms) => Instance.ThrottleUpdate(ms, () => Console.KeyAvailable);
		public Time() {
			_timer = new Stopwatch();
			_timer.Start();
			_timeSecOfCurrentFrame = _timer.Elapsed.TotalSeconds;
			_timeMsOfCurrentFrame = _timer.ElapsedMilliseconds;
			UpdateTiming();
		}
		public void UpdateTiming() {
			_deltaTimeMs = DeltaTimeMsCalculateNow;
			_deltaTimeSec = DeltaTimeSecCalculateNow;
			_timeSecOfCurrentFrame = _timer.Elapsed.TotalSeconds;
			_timeMsOfCurrentFrame = _timer.ElapsedMilliseconds;
		}
		public void ThrottleUpdate(int idealFrameDelayMs, Func<bool> interruptSleep = null) {
			long soon = _timeMsOfCurrentFrame + idealFrameDelayMs;
			while ((interruptSleep == null || !interruptSleep.Invoke()) && _timer.ElapsedMilliseconds < soon) {
				System.Threading.Thread.Sleep(1);
			}
		}
	}
}
