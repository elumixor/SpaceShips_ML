using System;
using MachineLearning.NN.Common;

namespace MachineLearning.NN.Neurons {
    [Serializable]
    public class InputNeuron {
        public float weight;
        public float bias;
        public float Apply(float value) => value * weight + bias;

        public static bool operator !=(InputNeuron a, InputNeuron b) => !(a == b);

        public static bool operator ==(InputNeuron a, InputNeuron b) => Math.Abs(a.weight - b.weight) < 1e-6f && Math.Abs(a.bias - b.bias) < 1e-6f;
    }
}