namespace MrV.GameEngine {
	public class Rand {
		private static Rand _instance;
		public static Rand Instance => _instance != null ? _instance : _instance = new Rand();
		public uint Seed = 2463534242; // seed (must be non-zero)
		/// <summary>Xorshift32</summary>
		uint Next() {
			Seed ^= Seed << 13;
			Seed ^= Seed >> 17;
			Seed ^= Seed << 5;
			return Seed;
		}
		private Rand() {}
		public static float Number => (Instance.Next() & 0xffffff) / (float)(0xffffff);
	}
}
