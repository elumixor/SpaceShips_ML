using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ML.NN {
    public enum ActivationFunction {
        Logistic,
        ReLU,
        LReLU,
        RReLU,
    }

    public static class ActivationFunctionsExtensions {
        public static float Apply(this ActivationFunction activationFunction, float value) {
            switch (activationFunction) {
                case ActivationFunction.Logistic: return 1f / (1f + Mathf.Exp(-value));
                case ActivationFunction.ReLU: return Mathf.Max(0, value);
                case ActivationFunction.LReLU: return Mathf.Max(0.1f * value, value);
                case ActivationFunction.RReLU: return Mathf.Max(Random.value * 0.5f * value, value);
                default: return value;
            }
        }
    }
}