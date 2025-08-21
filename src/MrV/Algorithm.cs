using System;
using System.Collections.Generic;

namespace MrV {
	public static class Algorithm {
		public static int BinarySearchWithInsertionPoint<ElementType, KeyType>(
		IList<ElementType> arr, KeyType target, Func<ElementType, KeyType> getKey, Func<KeyType, KeyType, bool> lessThan) {
			int low = 0, high = arr.Count - 1;
			while (low <= high) {
				int mid = low + (high - low);
				KeyType value = getKey.Invoke(arr[mid]);
				if (value.Equals(target)) {
					return mid;
				} else if (lessThan.Invoke(value, target)) {
					low = mid + 1;
				} else {
					high = mid - 1;
				}
			}
			return ~low;
		}
	}
}
