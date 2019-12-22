using System;
using UnityEngine;

namespace ML.NN {
    public enum ActivationFunction {
        None,
        Sigmoid
    }

    public static class ActivationFunctionsExtensions {
        public static float Apply(this ActivationFunction activationFunction, float value) {
            switch (activationFunction) {
                case ActivationFunction.Sigmoid: return 1f / (1f + Mathf.Exp(-value));
                default: return value;
            }
        }
    }
}