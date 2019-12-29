using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using ML.NN.Neurons;
using ML.ParameterFunctions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ML.NN {
    [Serializable]
    public class NeuralNetwork : IEnumerable<Neuron> {
        // neurons
        private readonly InputNeuron[] inputs;
        private readonly HiddenNeuron[][] hidden;
        private readonly Neuron[] outputs;

        // Fitness
        public float Fitness { get; set; }
        
        // genes
        public float[] genes;

        // Initialization

        /// <summary>
        /// Create new Neural Network by given <see cref="NetworkLayout"/>
        /// </summary>
        /// <param name="networkLayout">NN layout</param>
        public NeuralNetwork(NetworkLayout networkLayout) {
            var ic = networkLayout.inputs;
            var oc = networkLayout.outputs;
            var ih = networkLayout.hidden;

            inputs = new InputNeuron[ic];
            outputs = new Neuron[oc];
            hidden = new HiddenNeuron[ih.Length][];

            // Initialize DNA
            genes = new float[networkLayout.TotalGenes];

            // Gene counter
            var g = 0;


            // Generate input layer
            for (var i = 0; i < ic; i++) {
                var neuron = inputs[i] = new InputNeuron();

                foreach (var gene in neuron.Genes) genes[g++] = gene;
            }

            // Number of inputs for neuron in layer, which equals to number of neurons in previous layer
            var layerInputsCount = ic;

            // Generate hidden layers
            for (var i = 0; i < ih.Length; i++) {
                var hc = ih[i];
                var layer = hidden[i] = new HiddenNeuron[hc];

                for (var j = 0; j < hc; j++) {
                    var neuron = layer[j] = new HiddenNeuron(layerInputsCount, networkLayout.activationFunction);
                    foreach (var gene in neuron.Genes) genes[g++] = gene;
                }

                layerInputsCount = hc;
            }

            // Generate output layer
            for (var i = 0; i < oc; i++) {
                var neuron = outputs[i] = new Neuron(layerInputsCount);
                foreach (var gene in neuron.Genes) genes[g++] = gene;
            }
        }

        /// <summary>
        /// Sets random values onto NN's neurons' weights and biases
        /// </summary>
        public void SetRandomValues(float min = 0f, float max = 1f) {
            var g = 0;
            foreach (var neuron in this) {
                neuron.Randomize(min, max);

                foreach (var gene in neuron.Genes) genes[g++] = gene;
            }
        }

        // Application

        /// <summary>
        /// Feed forward inputs into NN
        /// </summary>
        /// <returns>Vector of output values</returns>
        public Vector Apply(Vector input) {
            var v = new Vector(inputs.Length);

            for (var i = 0; i < inputs.Length; i++) v.Values[i] = inputs[i].Apply(input.Values[0]);

            foreach (var layer in hidden) {
                var v1 = new Vector(layer.Length);

                for (var j = 0; j < layer.Length; j++) v1.Values[j] = layer[j].Apply(v);

                v = v1;
            }

            var v2 = new Vector(outputs.Length);

            for (var j = 0; j < outputs.Length; j++) v2.Values[j] = outputs[j].Apply(v);

            return v2;
        }

        /// <summary>
        /// Updates neurons wrt to current genes. Call this function after you change genes
        /// </summary>
        public void UpdateFromGenes() {
            var g = 0;
            foreach (var neuron in this) {
                for (var index = 0; index < neuron.Genes.Length; index++) neuron.Genes[index] = genes[g++];
                neuron.UpdateFromGenes();
            }
        }

        #region IEnumerable implementation

        public IEnumerator<Neuron> GetEnumerator() {
            foreach (var neuron in inputs) yield return neuron;

            foreach (var layer in hidden)
            foreach (var neuron in layer)
                yield return neuron;

            foreach (var neuron in outputs) yield return neuron;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}