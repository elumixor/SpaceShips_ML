namespace ML.NN.Neurons {
    public class InputNeuron : Neuron {
        public float Apply(float v) => weights.Values[0] * v + bias;

        public InputNeuron() : base(1) { }
    }
}