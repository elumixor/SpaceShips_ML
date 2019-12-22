using UnityEngine;

namespace MachineLearning.NN.ActivationFunctions {
    public class Sigmoid : ActivationFunction {
        public override float Apply(float x) => 1f / (1f + Mathf.Exp(-x));
    }
}