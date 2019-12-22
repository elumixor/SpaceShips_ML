using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common {
    [Serializable]
    public struct Vector {
        /// <summary>
        /// Values storage
        /// </summary>
        // ReSharper disable once Unity.RedundantAttributeOnTarget
        [field: SerializeField]
        public float[] Values { get; private set; }

        /// <summary>
        /// Vector's size
        /// </summary>
        public int Length => Values.Length;

        /// <summary>
        /// Create vector of specific length, filled with same float values
        /// </summary>
        public Vector(int size, float value = 0f) {
            Values = new float[size];
            for (var i = 0; i < size; i++) Values[i] = value;
        }

        /// <summary>
        /// Create vector from values
        /// </summary>
        /// <param name="values"></param>
        public Vector(params float[] values) {
            Values = values;
        }

        /// <summary>
        /// Dot product of two vectors
        /// </summary>
        /// <exception cref="Exception">If vectors have different length</exception>
        public static float operator *(Vector a, Vector b) {
            if (a.Length != b.Length) throw new Exception("Cannot multiply vectors of different length");

            var sum = 0f;
            for (var i = 0; i < a.Length; i++) sum += a.Values[i] * b.Values[i];
            return sum;
        }

        /// <summary>
        /// Scalar vector multiplication
        /// </summary>
        public static Vector operator *(Vector a, float value) {
            var result = new Vector();
            var len = a.Length;
            result.Values = new float[len];
            for (var i = 0; i < len; i++) result.Values[i] = a.Values[i] * value;

            return result;
        }
    }
}