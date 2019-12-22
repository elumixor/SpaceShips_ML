using System;
using System.Collections.Generic;
using System.Linq;
using MachineLearning.NN;
using MachineLearning.NN.ActivationFunctions;
using MachineLearning.NN.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MachineLearning.Generators {
    [CreateAssetMenu(fileName = "Genetic Generator", menuName = "Generators/Genetic")]
    public class GeneticGenerator : NetworkGenerator {
        [SerializeField] private List<int> hiddenLayers;

        public override NeuralNetwork Generate(int objectIndex, int inputCount, int outputCount) {
            var nn = new NeuralNetwork(ActivationFunction.Sigmoid, inputCount, outputCount, hiddenLayers);

            foreach (var inputNeuron in nn.inputs) {
                inputNeuron.weight = Random.Range(-1f, 1f);
                inputNeuron.bias = Random.Range(-1f, 1f);
            }

            foreach (var neuron in nn.hiddenLayers.SelectMany(hiddenLayer => hiddenLayer.hiddenLayer)) {
                neuron.weights = Vector.Random(neuron.weights.values.Count, -1, 1);
                neuron.bias = Random.Range(-1f, 1f);
            }

            foreach (var neuron in nn.outputs) {
                neuron.weights = Vector.Random(neuron.weights.values.Count, -1, 1);
                neuron.bias = Random.Range(-1f, 1f);
            }

            return nn;
        }

        public override NeuralNetwork RandomizeNN(NeuralNetwork baseNN, float deviation, float similarity) {
            var nn = new NeuralNetwork(ActivationFunction.Sigmoid, baseNN.inputs.Count, baseNN.outputs.Count, hiddenLayers);

            
            
            float Randomize(float v) {
                var v1 = Mathf.Lerp(v, Random.Range(-1f, 1f), Mathf.Lerp(0, Random.value * deviation, similarity));
//                return v * Random.Range(1f - deviation, 1f + deviation);    
                return v1;
            }

            for (var index = 0; index < nn.inputs.Count; index++) {
                var inputNeuron = nn.inputs[index];
                var inputNeuronBase = baseNN.inputs[index];
                inputNeuron.weight = Randomize(inputNeuronBase.weight);
                inputNeuron.bias = Randomize(inputNeuronBase.bias);
            }

            for (var i = 0; i < hiddenLayers.Count; i++) {
                var layerCount = hiddenLayers[i];
                
                var layer = nn.hiddenLayers[i].hiddenLayer;
                var layerBase = baseNN.hiddenLayers[i].hiddenLayer;
                
                for (var j = 0; j < layerCount; j++) {
                    layer[j].weights = layerBase[j].weights.values.Select(Randomize).ToVector();
                    layer[j].bias = Randomize(layerBase[j].bias);
                }
            }


            for (var index = 0; index < nn.outputs.Count; index++) {
                var neuron = nn.outputs[index];
                var neuronBase = baseNN.outputs[index];
                neuron.weights = neuronBase.weights.values.Select(Randomize).ToVector();
                neuron.bias = Randomize(neuronBase.bias);
            }

            return nn;
        }
    }
}