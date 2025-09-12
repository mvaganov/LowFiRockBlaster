using System;
using System.Collections.Generic;

namespace MrV.GameEngine {
	public class ObjectPool<T> {
		private List<T> allObjects = new List<T>();
		private int freeObjectCount = 0;
		public Func<T> CreateObject;
		public Action<T> DestroyObject, CommissionObject, DecommissionObject;
		private HashSet<int> delayedDecommission = new HashSet<int>();

		public int Count => allObjects.Count - freeObjectCount;
		public T this[int index] => index < Count
			? allObjects[index] : throw new ArgumentOutOfRangeException();
		public ObjectPool() { }
		public void Setup(Func<T> create, Action<T> commission = null,
			Action<T> decommission = null, Action<T> destroy = null) {
			CreateObject = create; CommissionObject = commission;
			DecommissionObject = decommission; DestroyObject = destroy;
		}
		public T Commission() {
			T freeObject = default;
			if (freeObjectCount == 0) {
				freeObject = CreateObject.Invoke();
				allObjects.Add(freeObject);
			} else {
				freeObject = allObjects[allObjects.Count - freeObjectCount];
				--freeObjectCount;
			}
			if (CommissionObject != null) { CommissionObject(freeObject); }
			return freeObject;
		}
		public void Decommission(T obj) => DecommissionAtIndex(allObjects.IndexOf(obj));
		public void DecommissionAtIndex(int indexOfObject) {
			if (indexOfObject >= (allObjects.Count - freeObjectCount)) {
				throw new Exception($"trying to free object twice: {allObjects[indexOfObject]}");
			}
			T obj = allObjects[indexOfObject];
			++freeObjectCount;
			int beginningOfFreeList = allObjects.Count - freeObjectCount;
			allObjects[indexOfObject] = allObjects[beginningOfFreeList];
			allObjects[beginningOfFreeList] = obj;
			if (DecommissionObject != null) { DecommissionObject.Invoke(obj); }
		}
		public void Clear() {
			for (int i = allObjects.Count - freeObjectCount - 1; i >= 0; --i) {
				Decommission(allObjects[i]);
			}
		}
		public void Dispose() {
			Clear();
			if (DestroyObject != null) { ForEach(DestroyObject.Invoke); }
			allObjects.Clear();
		}
		public void ForEach(Action<T> action) {
			for (int i = 0; i < allObjects.Count; ++i) {
				action.Invoke(allObjects[i]);
			}
		}
		public void DecommissionDelayedAtIndex(int indexOfObject) {
			delayedDecommission.Add(indexOfObject);
		}
		public void ServiceDelayedDecommission() {
			if (delayedDecommission.Count == 0) { return; }
			List<int> decommisionNow = new List<int>(delayedDecommission);
			decommisionNow.Sort();
			delayedDecommission.Clear();
			for (int i = decommisionNow.Count - 1; i >= 0; --i) {
				DecommissionAtIndex(decommisionNow[i]);
			}
		}
	}
}
