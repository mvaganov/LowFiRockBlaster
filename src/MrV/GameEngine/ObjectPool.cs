using System;
using System.Collections.Generic;

namespace MrV.GameEngine {
	public class ObjectPool<T> {
		private List<T> allObjects = new List<T>();
		private int freeObjectCount = 0;

		public DelegateCreate CreateDelegate;
		public DelegateDestroy DestroyDelegate;
		public DelegateCommission CommissionDelegate;
		public DelegateDecommission DecommissionDelegate;

		public delegate T DelegateCreate();
		public delegate void DelegateCommission(T obj);
		public delegate void DelegateDecommission(T obj);
		public delegate void DelegateDestroy(T obj);

		public int Count => allObjects.Count - freeObjectCount;
		public T this[int index] => index < Count
			? allObjects[index] : throw new ArgumentOutOfRangeException();
		public ObjectPool() { }
		public void Setup(DelegateCreate create, DelegateCommission commission = null,
			DelegateDecommission decommission = null, DelegateDestroy destroy = null) {
			CreateDelegate = create; CommissionDelegate = commission;
			DecommissionDelegate = decommission; DestroyDelegate = destroy;
		}
		public T Commission() {
			T freeObject = default;
			if (freeObjectCount == 0) {
				freeObject = CreateDelegate.Invoke();
				allObjects.Add(freeObject);
			} else {
				freeObject = allObjects[allObjects.Count - freeObjectCount];
				--freeObjectCount;
			}
			if (CommissionDelegate != null) { CommissionDelegate(freeObject); }
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
			if (DecommissionDelegate != null) { DecommissionDelegate.Invoke(obj); }
		}
		public void Clear() {
			for (int i = allObjects.Count - freeObjectCount - 1; i >= 0; --i) {
				Decommission(allObjects[i]);
			}
		}
		public void Destroy() {
			Clear();
			if (DestroyDelegate != null) { ForEach(DestroyDelegate.Invoke); }
			allObjects.Clear();
		}
		public void ForEach(Action<T> action) {
			for (int i = 0; i < allObjects.Count; ++i) {
				action.Invoke(allObjects[i]);
			}
		}
	}
}
