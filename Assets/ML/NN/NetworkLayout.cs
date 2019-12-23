using System.Linq;
using UnityEngine;

namespace ML.NN {
    [CreateAssetMenu(fileName = "NN Layout", menuName = "NN/Layout", order = 0)]
    public class NetworkLayout : ScriptableObject {
        public ActivationFunction activationFunction;
        public int inputs = GenerationInstance.InputsCount;
        public int outputs = GenerationInstance.OutputsCount;
        public int[] hidden;

        public int HiddenCount => hidden.Sum();
        public int Total => inputs + outputs + HiddenCount;

        public int TotalGenes {
            get {
                var sum = 0;
                sum += inputs * 2; // weight and bias for each input neuron

                var lastLayerCount = inputs;

                foreach (var layer in hidden) {
                    sum += inputs * (lastLayerCount + 1); // each neuron has weights from previous layer plus self bias 
                    lastLayerCount = layer;
                }

                sum += outputs * (lastLayerCount + 1);

                return sum;
            }
        }
    }
}