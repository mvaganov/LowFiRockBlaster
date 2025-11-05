using System;
using System.Collections.Generic;

namespace MrV.GameEngine {
	public class PolicyDrivenObjectPool<T> {
		private List<T> _allObjects = new List<T>();
		private int _freeObjectCount = 0;
		public Func<T> CreateObject;
		public Action<T> DestroyObject, CommissionObject, DecommissionObject;
		private SortedSet<int> _delayedDecommission = new SortedSet<int>();

		public int Count => _allObjects.Count - _freeObjectCount;
		public T this[int index] => index < Count
			? _allObjects[index] : throw new ArgumentOutOfRangeException();
		public PolicyDrivenObjectPool() { }
		public void Setup(Func<T> create, Action<T> commission = null,
			Action<T> decommission = null, Action<T> destroy = null) {
			CreateObject = create; CommissionObject = commission;
			DecommissionObject = decommission; DestroyObject = destroy;
		}
		public T Commission() {
			T freeObject = default;
			// if threading, `lock(_allObjects)` around the rest of this method until the return
			if (_freeObjectCount == 0) {
				freeObject = CreateObject.Invoke();
				_allObjects.Add(freeObject);
			} else {
				freeObject = _allObjects[_allObjects.Count - _freeObjectCount];
				--_freeObjectCount;
			}
			if (CommissionObject != null) { CommissionObject(freeObject); }
			return freeObject;
		}
		public IList<T> Commission(int count) {
			int startingIndex = _allObjects.Count - _freeObjectCount;
			T[] objects = new T[count];
			int countToCreate = count - _freeObjectCount;
			int index = 0;
			while (_freeObjectCount > 0 && index < count) {
				objects[index++] = _allObjects[_allObjects.Count - _freeObjectCount];
				--_freeObjectCount;
			}
			if (countToCreate > 0) {
				int targetBufferSize = _allObjects.Count + countToCreate;
				if (_allObjects.Capacity < targetBufferSize) {
					_allObjects.Capacity = targetBufferSize;
				}
				for (int i = 0; i < countToCreate; ++i) {
					T newObject = CreateObject.Invoke();
					_allObjects.Add(newObject);
					objects[index++] = newObject;
				}
			}
			if (CommissionObject != null) { Array.ForEach(objects, CommissionObject); }
			return objects;
		}
		public void Decommission(T obj) => DecommissionAtIndex(_allObjects.IndexOf(obj));
		public void DecommissionAtIndex(int indexOfObject) {
			if (indexOfObject >= (_allObjects.Count - _freeObjectCount)) {
				throw new Exception($"trying to free object twice: {_allObjects[indexOfObject]}");
			}
			if (_delayedDecommission.Count > 0) {
				DecommissionDelayedAtIndex(indexOfObject);
				return;
			}
			T obj = _allObjects[indexOfObject];
			// if threading, `lock(_allObjects)` around the rest of this method
			++_freeObjectCount;
			int beginningOfFreeList = _allObjects.Count - _freeObjectCount;
			_allObjects[indexOfObject] = _allObjects[beginningOfFreeList];
			_allObjects[beginningOfFreeList] = obj;
			if (DecommissionObject != null) { DecommissionObject.Invoke(obj); }
		}
		public void Clear() {
			for (int i = _allObjects.Count - _freeObjectCount - 1; i >= 0; --i) {
				Decommission(_allObjects[i]);
			}
		}
		public void Dispose() {
			Clear();
			if (DestroyObject != null) { ForEach(DestroyObject.Invoke); }
			_allObjects.Clear();
		}
		public void ForEach(Action<T> action) {
			for (int i = 0; i < _allObjects.Count; ++i) {
				action.Invoke(_allObjects[i]);
			}
		}
		public void DecommissionDelayedAtIndex(int indexOfObject) {
			_delayedDecommission.Add(indexOfObject);
		}
		public void ServiceDelayedDecommission() {
			if (_delayedDecommission.Count == 0) { return; }
			List<int> decommissionNow = new List<int>(_delayedDecommission);
			_delayedDecommission.Clear();
			for (int i = decommissionNow.Count - 1; i >= 0; --i) {
				DecommissionAtIndex(decommissionNow[i]);
			}
		}
	}
}
