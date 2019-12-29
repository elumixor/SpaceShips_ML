using ML.NN;
using UnityEngine;

namespace ML.ParameterFunctions {
    public enum MutationFunction {
        Relative,
        Absolute,
        None,
    }

    public static class MutationFunctionExtensions {
        private static bool DoMutation(float probability) => Random.value < probability;

        public static void Mutate(this MutationFunction mutationFunction, NeuralNetwork child, float probability, float factor) {
            switch (mutationFunction) {
                case MutationFunction.Absolute:
                    for (var i = 0; i < child.genes.Length; i++) {
                        var gene = child.genes[i];
                        child.genes[i] = DoMutation(probability) ? gene : gene + Random.Range(-factor, factor);
                    }


                    break;

                case MutationFunction.Relative:
                    for (var i = 0; i < child.genes.Length; i++) {
                        var gene = child.genes[i];
                        child.genes[i] = DoMutation(probability) ? gene : gene + gene * Random.Range(-factor, factor);
                    }

                    break;

                case MutationFunction.None:
                default:
                    break;
            }
        }
    }
}