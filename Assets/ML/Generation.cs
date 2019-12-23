using ML.NN;
using UnityEngine;

namespace ML {
    /// <summary>
    /// Manages current generation 
    /// </summary>
    public class Generation : MonoBehaviour {
        private float lifetime;

        /// <summary>
        /// Lifetime of this generation
        /// </summary>
        public float Lifetime {
            get => lifetime;
            set {
                lifetime = value;
                foreach (var instance in Instances) instance.Lifetime = lifetime;
            }
        }

        /// <summary>
        /// Created instances
        /// </summary>
        public GenerationInstance[] Instances { get; private set; }

        /// <summary>
        /// Generation number
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// How much are genes mutated (0 = none at all, 1 = completely)
        /// </summary>
        public float MutationFactor { get; private set; }

        /// <summary>
        /// How high is the probability of mutation (0 = never, 1 = always)
        /// </summary>
        public float MutationProbability { get; private set; }

        /// <summary>
        /// Factory for creating random generation
        /// </summary>
        /// <param name="lifetime">Lifetime of an instance in the generation </param>
        /// <param name="instancesCount">Number of instances in generation</param>
        /// <param name="mutationProbability">How often are genes mutated</param>
        /// <param name="mutationFactor">How much much do genes deviate when mutated</param>
        public static Generation Random(float lifetime, int instancesCount, float mutationProbability, float mutationFactor) {
            var gen = new GameObject("Generation 0").AddComponent<Generation>();

            gen.lifetime = lifetime;
            gen.MutationFactor = mutationFactor;
            gen.MutationProbability = mutationProbability;

            gen.CreateInstances(instancesCount);

            foreach (var instance in gen.Instances) instance.NN.SetRandomValues(-1, 1);

            return gen;
        }

        /// <summary>
        /// Factory to reproduce generation 
        /// </summary>
        /// <param name="lifetime">Lifetime of an instance in the generation </param>
        /// <param name="instancesCount">Number of instances in generation</param>
        /// <param name="mutationProbability">How often are genes mutated</param>
        /// <param name="mutationFactor">How much much do genes deviate when mutated</param>
        /// <param name="evaluation">Generation evaluation. Create via <see cref="Evaluate"/></param>
        /// <param name="newRandomCount">Number of completely new instances</param>
        /// <param name="generationNumber"></param>
        /// <returns></returns>
        public static Generation Reproduce(float lifetime, int instancesCount, float mutationProbability, float mutationFactor,
            GenerationEvaluation evaluation, int newRandomCount, int generationNumber) {
            var gen = new GameObject($"Generation {generationNumber}").AddComponent<Generation>();
            gen.Number = generationNumber;

            gen.lifetime = lifetime;
            gen.MutationFactor = mutationFactor;
            gen.MutationProbability = mutationProbability;

            gen.CreateInstances(instancesCount);

            for (var index = 0; index < gen.Instances.Length; index++) {
                var instance = gen.Instances[index];
                if (index < newRandomCount) instance.NN.SetRandomValues(-1, 1);
                else {
                    instance.NN.Crossover(evaluation);
                    instance.NN.Mutate(gen.MutationProbability, gen.MutationFactor);
                }
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
        /// <param name="count"></param>
        private void CreateInstances(int count) {
            Instances = new GenerationInstance[count];
            for (var i = 0; i < count; i++) {
                var instance = Instances[i] = Instantiate(MainHandler.InstancePrefab, transform);
                instance.Lifetime = lifetime;
            }
        }
    }
}