using System.Collections.Generic;
using System.Linq;
using ML.NN;
using ML.ParameterFunctions;
using UnityEngine;

namespace ML {
    public class MatingPool {
        public class Parent {
            public float Fitness { get; }
            public float[] Genes { get; }

            public Parent(float fitness, float[] genes) {
                Fitness = fitness;
                Genes = genes;
            }

            public void Deconstruct(out float fitness, out float[] genes) {
                fitness = Fitness;
                genes = Genes;
            }
        }

        public readonly Parent[] elements;

        public MatingPool(IReadOnlyCollection<GenerationInstance> instances) {
            var count = instances.Count;
            var ordered = instances.OrderBy(i => i.NN.Fitness).ToArray();

            var fitnessMaximum = ordered[count - 1].NN.Fitness;
            var fitnessMinimum = ordered[0].NN.Fitness;

            // normalized fitness
            elements = ordered.Select(o => new Parent((o.NN.Fitness - fitnessMinimum) / (fitnessMaximum - fitnessMinimum), o.NN.genes))
                .ToArray();
        }

        public Parent Best(int offset = 0) => elements[elements.Length - 1 - offset];
        public Parent Worst(int offset = 0) => elements[offset];
    }
}