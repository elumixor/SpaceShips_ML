using UnityEngine;

namespace ML {
    /// <summary>
    /// Manages current generation 
    /// </summary>
    public class Generation : MonoBehaviour {
        /// <summary>
        /// Lifetime of this generation
        /// </summary>
        private float lifetime;

        /// <summary>
        /// Current evaluation
        /// </summary>
        public readonly GenerationEvaluation evaluation = new GenerationEvaluation();

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

            foreach (var instance in gen.Instances) instance.NN.SetRandomValues();

            return gen;
        }

        /// <summary>
        /// Factory to reproduce generation 
        /// </summary>
        /// <param name="generation"></param>
        /// <returns></returns>
        public static Generation Reproduce(Generation generation) {
            var number = generation.Number + 1;
            var gen = new GameObject($"Generation {number}").AddComponent<Generation>();
            gen.Number = number;

            gen.lifetime = generation.lifetime;
            gen.MutationFactor = generation.MutationFactor;
            gen.MutationProbability = generation.MutationProbability;
            gen.CreateInstances(gen.Instances.Length);


            foreach (var instance in gen.Instances) {
                var nn = instance.NN;
                nn.Crossover(generation);
                nn.Mutate(gen.MutationProbability, gen.MutationFactor);
            }

            return gen;
        }

        /// <summary>
        /// Update current generation evaluation
        /// </summary>
        private void Update() {
            evaluation.Update(Instances);
        }

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