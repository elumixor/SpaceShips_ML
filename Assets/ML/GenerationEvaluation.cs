using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using ML.NN;
using ML.ParameterFunctions;
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
        public readonly MatingPool pool;

        public float[] Fitnesses { get; }


        /// <summary>
        /// Create generation evaluation.
        /// </summary>
        /// <remarks>
        /// Generates MatingPool
        /// </remarks>
        /// <param name="instances"></param>
        public GenerationEvaluation(IReadOnlyCollection<GenerationInstance> instances) {
            var count = instances.Count;
            var ordered = instances.OrderBy(i => i.NN.Fitness).ToArray();

            Fitnesses = ordered.Select(o => o.NN.Fitness).ToArray();
            
            FitnessMaximum = ordered[count - 1].NN.Fitness;
            FitnessMinimum = ordered[0].NN.Fitness;

            var sum = ordered.Sum(o => o.NN.Fitness);
            FitnessAverage = sum / count;
            FitnessMedian = (ordered[count / 2].NN.Fitness + ordered[(count - 1) / 2].NN.Fitness) * .5f;

            pool = new MatingPool(instances);
        }
    }
}