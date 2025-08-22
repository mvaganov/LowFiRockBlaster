using System;
using System.Collections.Generic;

namespace MrV {
	public static class Algorithm {
		public static int BinarySearch<ElementType, KeyType>(IList<ElementType> arr, KeyType target,
		Func<ElementType, KeyType> getKey) where KeyType : IComparable {
			int low = 0, high = arr.Count - 1;
			while (low <= high) {
				int mid = low + (high - low);
				KeyType value = getKey.Invoke(arr[mid]);
				int comparison = value.CompareTo(target);
				if (comparison == 0) {
					return mid;
				} else if (comparison < 0) {
					low = mid + 1;
				} else {
					high = mid - 1;
				}
			}
			return ~low;
		}
	}
}
