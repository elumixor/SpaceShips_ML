using System;
using Common;

namespace ML.NN.Neurons {
    [Serializable]
    public class HiddenNeuron : Neuron {
        private readonly ActivationFunction activationFunction;

        public HiddenNeuron(int inputsCount, ActivationFunction activationFunction) : base(inputsCount) {
            this.activationFunction = activationFunction;
        }

        public HiddenNeuron(float[] genes, ActivationFunction activationFunction) : base(genes) {
            this.activationFunction = activationFunction;
        }

        public override float Apply(Vector v) => activationFunction.Apply(base.Apply(v));
    }
}