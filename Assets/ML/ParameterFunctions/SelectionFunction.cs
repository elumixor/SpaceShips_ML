using System.Collections.Generic;
using System.Linq;
using ML.NN;
using UnityEngine;
using Parents = System.ValueTuple<ML.NN.NeuralNetwork, ML.NN.NeuralNetwork>;

namespace ML.ParameterFunctions {
    public enum SelectionFunction {
        Best,
        TopTwo,
        TopTwoRandom,
    }

    public static class SelectionFunctionExtensions {
        public static (MatingPool.Parent better, MatingPool.Parent worse) Select(this MatingPool pool, SelectionFunction selectionFunction,
            float fitnessPower) {
            switch (selectionFunction) {
                case SelectionFunction.Best:
                    var b = pool.Best();
                    return (b, b);
                case SelectionFunction.TopTwo:
                    return (pool.Best(), pool.Best(1));
                case SelectionFunction.TopTwoRandom:
                    var r1 = Random.value;
                    var r2 = Random.value;

                    var (max, min) = r1 > r2 ? (r1, r2) : (r2, r1);

                    return (pool.elements.First(p => Mathf.Pow(p.Fitness, fitnessPower) >= max),
                        pool.elements.First(p => Mathf.Pow(p.Fitness, fitnessPower) >= min));
                default:
                    return default;
            }
        }
    }
}