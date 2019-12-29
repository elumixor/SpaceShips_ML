using System.Threading;
using ML.NN;
using ML.ParameterFunctions;
using UnityEngine;

namespace ML {
    /// <summary>
    /// Manages current generation 
    /// </summary>
    public class Generation {
        /// <summary>
        /// Game Object, containing generation instances
        /// </summary>
        public GameObject GameObject;

        /// <summary>
        /// Created instances
        /// </summary>
        public GenerationInstance[] Instances { get; private set; }

        /// <summary>
        /// Generation number
        /// </summary>
        public int Number { get; }

        
        private Generation(int generationNumber, int instancesCount) {
            GameObject = new GameObject($"Generation {generationNumber}");
            Number = generationNumber;
            CreateInstances(instancesCount);
        }

        /// <summary>
        /// Factory for creating random generation
        /// </summary>
        public static Generation Random(GenerationParameters parameters) {
            var gen = new Generation(0, parameters.instancesCount);

            foreach (var instance in gen.Instances) {
                instance.NN.SetRandomValues(-1, 1);
                instance.dieOnCollision = parameters.dieOneCollision;
            }

            return gen;
        }

        /// <summary>
        /// Factory to reproduce generation 
        /// </summary>
        /// <param name="parameters">Generation parameters</param>
        /// <param name="evaluation">Generation evaluation. Create via <see cref="Evaluate"/></param>
        /// <param name="generationNumber"></param>
        /// <returns></returns>
        public static Generation Reproduce(GenerationParameters parameters, GenerationEvaluation evaluation, int generationNumber) {
            var gen = new Generation(generationNumber, parameters.instancesCount);

            for (var index = 0; index < gen.Instances.Length; index++) {
                var instance = gen.Instances[index];

                if (index < parameters.preservedCount) {
                    instance.NN.genes = evaluation.pool.Best().Genes;
                    instance.NN.UpdateFromGenes();
                }
                else if (index < parameters.preservedCount + parameters.newRandomCount) instance.NN.SetRandomValues(-1, 1);
                else {
                    parameters.crossoverFunction.Crossover(instance.NN, evaluation, parameters.selectionFunction, parameters.fitnessPower);
                    parameters.mutationFunction.Mutate(instance.NN, parameters.mutationProbability, parameters.mutationFactor);
                    
                    instance.NN.UpdateFromGenes();
                }

                instance.dieOnCollision = parameters.dieOneCollision;
                instance.fitnessFunction = parameters.fitnessFunction;
            }

            return gen;
        }

        /// <summary>
        /// Evaluate current generation
        /// </summary>
        /// <remarks>
        /// Generates Mating Pool
        /// </remarks>
        /// <returns></returns>
        public GenerationEvaluation Evaluate() => new GenerationEvaluation(Instances);

        /// <summary>
        /// Helper function to create and initialize instances
        /// </summary>
        private void CreateInstances(int count) {
            Instances = new GenerationInstance[count];
            for (var i = 0; i < count; i++) Instances[i] = Object.Instantiate(MainHandler.InstancePrefab, GameObject.transform);
        }
    }
}