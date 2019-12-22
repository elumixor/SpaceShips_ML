using System;
using MachineLearning.NN.Common;
using UnityEngine;

namespace MachineLearning.NN.Neurons {
    [Serializable]
    public class OutputNeuron {
        public Vector weights;
        public float bias;
        
        public float Apply(Vector values) => weights * values + bias;
        
        
        public static bool operator !=(OutputNeuron a, OutputNeuron b) => !(a == b);

        public static bool operator ==(OutputNeuron a, OutputNeuron b) => a.weights == b.weights && Math.Abs(a.bias - b.bias) < 1e-6f;
    }
}