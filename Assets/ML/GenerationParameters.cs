using System;
using ML.ParameterFunctions;
using UnityEngine;

namespace ML {
    [Serializable]
    public struct GenerationParameters {
        /// <summary>
        /// Number of instances in this generation
        /// </summary>
        public int instancesCount;

        /// <summary>
        /// Number of randomly generated instances (ignoring crossover)
        /// </summary>
        public int newRandomCount;

        /// <summary>
        /// Number of instances, that are preserved
        /// </summary>
        public int preservedCount;

        /// <summary>
        /// Lifetime of a generation's instance
        /// </summary>
        [Range(0, 10)] public float lifetime;

        /// <summary>
        /// Probability of mutation, for a gene
        /// </summary>
        [Range(0, 1)] public float mutationProbability;

        /// <summary>
        /// Percentage of gene's value to be either added or subtracted
        /// </summary>
        [Range(0, 1)] public float mutationFactor;

        /// <summary>
        /// Raise relative fitness to power, higher values make good fitness score better, and bat fitness score worse
        /// </summary>
        [Range(0, 32)] public float fitnessPower;

        /// <summary>
        /// If true will die on collision with obstacles
        /// </summary>
        public bool dieOneCollision;
        
        // Parameter functions

        /// <summary>
        /// Fitness function
        /// </summary>
        public FitnessFunction fitnessFunction;

        /// <summary>
        /// Mutation function
        /// </summary>
        public MutationFunction mutationFunction;

        /// <summary>
        /// Crossover function
        /// </summary>
        public CrossoverFunction crossoverFunction;

        /// <summary>
        /// Selection function
        /// </summary>
        public SelectionFunction selectionFunction;
    }
}