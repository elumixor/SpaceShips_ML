using System;
using Common;
using Random = UnityEngine.Random;

namespace ML.NN.Neurons {
    [Serializable]
    public class Neuron {
        public Vector weights;
        public float bias;

        public float[] Genes { get; private set; }

        /// <summary>
        /// Create neuron with weights and biases set to zero
        /// </summary>
        /// <param name="inputsCount"></param>
        public Neuron(int inputsCount) {
            weights = new Vector(inputsCount);
            Genes = new float[inputsCount + 1]; // extra 1 for bias
        }

        /// <summary>
        /// Create neuron from genes
        /// </summary>
        /// <param name="genes">Sequence of genes for weights and biases</param>
        public Neuron(params float[] genes) {
            Genes = genes;

            var count = genes.Length;

            weights = new Vector(count - 1);

            var i = 0;
            for (; i < count - 1; i++) {
                weights.Values[i] = genes[i];
            }

            bias = genes[i];
        }

        public virtual float Apply(Vector v) => weights * v + bias;

        public void Randomize(float min = 0f, float max = 1f) {
            for (var i = 0; i < weights.Length; i++) Genes[i] = weights.Values[i] = Random.Range(min, max);
            Genes[weights.Length] = bias = Random.Range(min, max);
        }

        /// <summary>
        /// Updates neuron's weights and biases wrt to current genes. Call this function after you change genes
        /// </summary>
        public void UpdateFromGenes() {
            for (var i = 0; i < weights.Length; i++) weights.Values[i] = Genes[i];
            bias = Genes[weights.Length];
        }
    }
}