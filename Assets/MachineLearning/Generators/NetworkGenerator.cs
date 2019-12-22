using System.Collections.Generic;
using MachineLearning.NN;
using MachineLearning.NN.ActivationFunctions;
using UnityEngine;

namespace MachineLearning.Generators {
    public abstract class NetworkGenerator : ScriptableObject {
        public abstract NeuralNetwork Generate(int objectIndex, int inputCount, int outputCount);

        public abstract NeuralNetwork RandomizeNN(NeuralNetwork neuralNetwork, float deviation, float similarity);
    }
}