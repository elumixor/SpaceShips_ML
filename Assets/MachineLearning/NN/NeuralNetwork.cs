using System;
using System.Collections.Generic;
using System.Linq;
using MachineLearning.NN.ActivationFunctions;
using MachineLearning.NN.Common;
using MachineLearning.NN.Neurons;
using UnityEngine;

namespace MachineLearning.NN {
    [Serializable]
    public class NeuralNetwork {
        [SerializeField] public List<InputNeuron> inputs;
        [SerializeField] public List<HiddenLayer> hiddenLayers;
        [SerializeField] public List<OutputNeuron> outputs;

        [Serializable]
        public class HiddenLayer {
            public List<HiddenNeuron> hiddenLayer;
            public HiddenLayer(int neuronsInLayer) => hiddenLayer = new List<HiddenNeuron>(neuronsInLayer);
        }

        public NeuralNetwork(ActivationFunction activationFunction, int inputCount, int outputCount, IReadOnlyList<int> hiddenLayers) {
            inputs = new List<InputNeuron>(inputCount);
            for (var i = 0; i < inputCount; i++) inputs.Add(new InputNeuron());

            var count = inputCount;

            this.hiddenLayers = new List<HiddenLayer>();

            foreach (var neuronsInLayer in hiddenLayers) {
                var layer = new HiddenLayer(neuronsInLayer);
                this.hiddenLayers.Add(layer);

                for (var j = 0; j < neuronsInLayer; j++)
                    layer.hiddenLayer.Add(new HiddenNeuron {
                        activationFunction = activationFunction,
                        weights = new Vector(count)
                    });

                count = neuronsInLayer;
            }

            outputs = new List<OutputNeuron>(inputCount);
            for (var i = 0; i < outputCount; i++) outputs.Add(new OutputNeuron {weights = new Vector(count)});
        }

        public Vector Apply(Vector input) {
            var c = inputs.Count;
            Debug.Assert(input.values.Count == c);

            var i1 = new Vector(c);

            for (var i = 0; i < c; i++) i1.values[i] = inputs[i].Apply(input.values[i]);

            return FeedForward(i1, 0);
        }

        private Vector FeedForward(Vector previousLayer, int layerIndex) {
            var isHiddenLayer = layerIndex < hiddenLayers.Count;

            return isHiddenLayer
                ? FeedForward(hiddenLayers[layerIndex].hiddenLayer.Select(l => l.Apply(previousLayer)).ToVector(), layerIndex + 1)
                : outputs.Select((o, i) => o.Apply(previousLayer)).ToVector();
        }

        public bool Same(NeuralNetwork nn) {
            var ic = inputs.Count;
            var oc = outputs.Count;
            var hc = hiddenLayers.Count;

            if (ic != nn.inputs.Count) return false;
            if (oc != nn.outputs.Count) return false;
            if (hc != nn.hiddenLayers.Count) return false;

            for (var i = 0; i < ic; i++)
                if (inputs[i] != nn.inputs[i])
                    return false;
            
            for (var i = 0; i < oc; i++)
                if (outputs[i] != nn.outputs[i])
                    return false;

            for (var i = 0; i < hc; i++) {
                var hl1 = hiddenLayers[i].hiddenLayer;
                var hl2 = nn.hiddenLayers[i].hiddenLayer;

                var hci = hl1.Count;
                if (hci != hl2.Count) return false;

                for (var j = 0; j < hci; j++)
                    if (hl1[j] != hl2[j])
                        return false;
            }

            return true;
        }
    }
}