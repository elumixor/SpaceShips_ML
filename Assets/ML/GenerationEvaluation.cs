using System;
using System.Collections.Generic;
using System.Linq;
using ML.NN;
using Random = UnityEngine.Random;

namespace ML {
    public class GenerationEvaluation {
        public float FitnessMaximum { get; private set; }
        public float FitnessAverage { get; private set; }
        public float FitnessMedian { get; private set; }
        public float FitnessMinimum { get; private set; }

        /// <summary>
        /// Ordered array of neural networks, generated randomly, wrt fitnesses. (first = best, last = worst) 
        /// </summary>
        public GenerationInstance[] MatingPool { get; private set; }

        public void Update(GenerationInstance[] instances) {
            var count = instances.Length;
            var ordered = instances.OrderBy(i => i.Fitness).Select(i => i.Fitness).ToArray();

            FitnessMaximum = ordered[count - 1];
            FitnessMinimum = ordered[0];

            FitnessAverage = ordered.Sum() / instances.Length;
            FitnessMedian = (ordered[count / 2] + ordered[(count - 1) / 2]) / 2;

            MatingPool = instances
                .OrderByDescending(i => (i.Fitness - FitnessMinimum) / (FitnessMaximum - FitnessMinimum) * Random.value)
                .ToArray();
        }
    }
}