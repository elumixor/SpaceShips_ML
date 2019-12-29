using System;
using System.Diagnostics.CodeAnalysis;
using ML.NN;
using Random = UnityEngine.Random;


namespace ML.ParameterFunctions {
    public enum CrossoverFunction {
        Step,
        Average,
        HalfBestWorse,
        HalfWorstBest,
        HalfRandomShift,
        StepRandom,
        FractionByFitness,
        FractionRandom,
    }

    public static class CrossoverExtensions {
        [SuppressMessage("ReSharper", "UseDeconstruction")]
        public static void Crossover(this CrossoverFunction crossoverFunction, NeuralNetwork child,
            GenerationEvaluation evaluation, SelectionFunction selectionFunction, float fitnessPower) {
            var (b, w) = evaluation.pool.Select(selectionFunction, fitnessPower);
            var (better, worse) = (b.Genes, w.Genes);

            var genes = child.genes;
            var count = genes.Length;
            var halfCount = genes.Length / 2;

            switch (crossoverFunction) {
                case CrossoverFunction.HalfWorstBest:
                    for (var i = 0; i < halfCount; i++) child.genes[i] = better[i];
                    for (var i = halfCount; i < count; i++) child.genes[i] = worse[i];
                    break;

                case CrossoverFunction.HalfBestWorse:
                    for (var i = 0; i < halfCount; i++) child.genes[i] = worse[i];
                    for (var i = halfCount; i < count; i++) child.genes[i] = better[i];
                    break;

                case CrossoverFunction.HalfRandomShift:
                    var shift = Random.Range(0, count - 1);
                    var overlaps = shift + halfCount > count - 1;
                    
                    for (var i = 0; i < count; i++) child.genes[i] = (!overlaps && i > shift && i < shift + halfCount || overlaps && !(i > shift && i < shift + halfCount) ? better : worse)[i];
                    break;

                case CrossoverFunction.Step:
                    for (var i = 0; i < count; i++) genes[i] = (i % 2 == 0 ? better : worse)[i];
                    break;

                case CrossoverFunction.StepRandom:
                    for (var i = 0; i < count; i++) genes[i] = (Random.value > .5f ? better : worse)[i];
                    break;

                case CrossoverFunction.FractionRandom: {
                    for (var i = 0; i < count; i++) {
                        var fraction = Random.value;
                        genes[i] = better[i] * fraction + worse[i] * (1f - fraction);
                    }

                    break;
                }

                case CrossoverFunction.FractionByFitness: {
                    var fraction = b.Fitness / (b.Fitness + w.Fitness);
                    for (var i = 0; i < count; i++) genes[i] = better[i] * fraction + worse[i] * (1f - fraction);

                    break;
                }

                case CrossoverFunction.Average:
                default:
                    for (var i = 0; i < count; i++) genes[i] = (better[i] + worse[i]) * .5f;
                    break;
            }
        }
    }
}