using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using ML.NN;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ML {
    public class GenerationEvaluation {
        public float FitnessMaximum { get; }

        public float FitnessAverage { get; }

        public float FitnessMedian { get; }

        public float FitnessMinimum { get; }

        /// <summary>
        /// Ordered array of tuples of relative normalized fitnesses and corresponding nn.
        /// </summary>
        private readonly (float Fitness, NeuralNetwork NN)[] pool;

        public NeuralNetwork NNFromMatingPool {
            get {
                var r = Random.value;
                
                return pool.First(p => p.Fitness >= r).NN;
            }
        }

        /// <summary>
        /// Create generation evaluation.
        /// </summary>
        /// <remarks>
        /// Generates MatingPool
        /// </remarks>
        /// <param name="instances"></param>
        public GenerationEvaluation(IReadOnlyCollection<GenerationInstance> instances) {
            var count = instances.Count;
            var ordered = instances.OrderBy(i => i.Fitness).ToArray();

            FitnessMaximum = ordered[count - 1].Fitness;
            FitnessMinimum = ordered[0].Fitness;

            var sum = ordered.Sum(o => o.Fitness);
            FitnessAverage = sum / count;
            FitnessMedian = (ordered[count / 2].Fitness + ordered[(count - 1) / 2].Fitness) * .5f;

            var s = 0f;

            pool = new (float Fitness, NeuralNetwork NN)[count];
            for (var i = 0; i < ordered.Length; i++) {
                var o = ordered[i];
                s += (o.Fitness - FitnessMinimum) / (sum - FitnessMinimum * count);
                pool[i] = (s, o.NN);
            }
        }
    }
}