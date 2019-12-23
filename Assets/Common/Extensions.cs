using System.Collections.Generic;
using UnityEngine;

namespace Common {
    public static class Extensions {
        public static void Log<T>(this IEnumerable<T> enumerable) {
            foreach (var element in enumerable) Debug.Log(element);
        }
    }
}