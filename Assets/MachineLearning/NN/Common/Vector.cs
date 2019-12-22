using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MachineLearning.NN.Common {
    [Serializable]
    public class Vector {
        public List<float> values;

        public Vector(int capacity) {
            values = new List<float>(capacity);
            for (var i = 0; i < capacity; i++) values.Add(0f);
        }

        public Vector(params float[] v) {
            values = new List<float>(v.Length);
            foreach (var f in v) values.Add(f);
        }

        public Vector() {
            values = new List<float>();
        }

        public static float operator *(Vector a, Vector b) => a.values.Select((t, i) => t * b.values[i]).Sum();
        public static float operator *(Vector a, float b) => a.values.Select(t => t * b).Sum();

        public static Vector Random(int size) {
            var v = new Vector();
            for (var i = 0; i < size; i++) v.values.Add(UnityEngine.Random.value);
            return v;
        }

        public static Vector OfValue(int size, float value) {
            var v = new Vector();
            for (var i = 0; i < size; i++) v.values.Add(value);
            return v;
        }

        public override string ToString() => string.Join(", ", values);

        public static Vector Random(int size, float min, float max) {
            var v = new Vector();
            for (var i = 0; i < size; i++) v.values.Add(UnityEngine.Random.Range(min, max));
            return v;
        }

        public static bool operator ==(Vector a, Vector b) {
            if (a.values.Count != b.values.Count) return false;

            return !a.values.Where((t, i) => Math.Abs(t - b.values[i]) > 1e-6f).Any();
        }

        public static bool operator !=(Vector a, Vector b) => !(a == b);
    }

    public static class VectorExtensions {
        public static Vector ToVector(this IEnumerable<float> enumerable) {
            var v = new Vector();
            v.values.AddRange(enumerable);
            return v;
        }
    }
}