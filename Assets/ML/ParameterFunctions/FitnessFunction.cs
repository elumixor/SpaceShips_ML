using System;
using UnityEngine;

namespace ML.ParameterFunctions {
    public enum FitnessFunction {
        Linear,
        Gate,
        VerticalWithGates,
    }

    public static class FitnessExtensions {
        public static float EvaluateFitness(this FitnessFunction fitnessFunction, GenerationInstance instance) {
            var f = instance.dieOnCollision && instance.Collided ? .5f : 1f;

            float val;
            switch (fitnessFunction) {
                case FitnessFunction.Linear:
                    val = instance.transform.position.y;
                    break;
                case FitnessFunction.Gate:
                    val = instance.transform.position.y * (Array.IndexOf(MainHandler.Gates, instance.currentGate) + 1);
                    break;
                case FitnessFunction.VerticalWithGates:
                    var goodDistance = instance.transform.position.y;
                    var wasteDistance = instance.totalDistance - goodDistance;

                    // we want to credit those, who move upwards
                    // but we dont want them to just stand
                    val = Mathf.Max(0, goodDistance * (Array.IndexOf(MainHandler.Gates, instance.currentGate) + 1) - wasteDistance);
                    break;
                default: return 0f;
            }

            return val * f;
        }
    }
}