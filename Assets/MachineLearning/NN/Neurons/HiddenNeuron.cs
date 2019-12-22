using System;
using MachineLearning.NN.ActivationFunctions;
using MachineLearning.NN.Common;
using UnityEngine;

namespace MachineLearning.NN.Neurons {
    [Serializable]
    public class HiddenNeuron {
        public Vector weights;
        public float bias;
        
        public ActivationFunction activationFunction = ActivationFunction.None;

        public virtual float Apply(Vector values) => weights * values + bias;
        
        public static bool operator !=(HiddenNeuron a, HiddenNeuron b) => !(a == b);

        public static bool operator ==(HiddenNeuron a, HiddenNeuron b) => a.weights == b.weights && Math.Abs(a.bias - b.bias) < 1e-6f;
    }
}