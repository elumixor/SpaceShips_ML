using System.Linq;
using UnityEngine;

namespace ML.NN {
    [CreateAssetMenu(fileName = "NN Layout", menuName = "NN/Layout", order = 0)]
    public class NetworkLayout : ScriptableObject {
        public ActivationFunction activationFunction;
        public int inputs = GenerationInstance.InputsCount;
        public int outputs = GenerationInstance.OutputsCount;
        public int[] hidden;

        public int HiddenCount => hidden.Sum();
        public int Total => inputs + outputs + HiddenCount;
    }
}