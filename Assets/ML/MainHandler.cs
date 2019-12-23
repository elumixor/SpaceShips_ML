using System.Collections;
using System.IO;
using System.Linq;
using ML.NN;
using UnityEngine;

namespace ML {
    /// <summary>
    /// Main class for handling generation creation and reproduction
    /// </summary>
    public class MainHandler : MonoBehaviour {
        // Parameters

        [SerializeField] private GenerationInstance instancePrefab;
        [SerializeField] private NetworkLayout networkLayout;

        [SerializeField] private int instancesCount;
        [SerializeField] private int newRandomCount;

        [SerializeField, Range(0, 10)] private float generationLifetime;
        [SerializeField, Range(0, 10)] private float simulationSpeed;
        [SerializeField, Range(0, 1)] private float mutationProbability;
        [SerializeField, Range(0, 1)] private float mutationFactor;
        [SerializeField] private string logFilePath;

        /// <summary>
        /// Instance prefab (set on Awake)
        /// </summary>
        public static GenerationInstance InstancePrefab { get; private set; }

        /// <summary>
        /// Layout of the NN of the neuron (set on Awake)
        /// </summary>
        public static NetworkLayout NetworkLayout { get; private set; }

        /// <summary>
        /// Array of gates in the scene, sorted vertically
        /// </summary>
        public static Gate.Gate[] Gates { get; private set; }

        // Publicly available data

        /// <summary>
        /// Time when last generation was created
        /// </summary>
        public float GenerationCreationTime { get; private set; }

        /// <summary>
        /// Time elapsed since last generation creation
        /// </summary>
        public float GenerationCurrentLifeTime => Time.time - GenerationCreationTime;

        /// <summary>
        /// Currently alive generation
        /// </summary>
        public Generation CurrentGeneration { get; private set; }

        private string FilePath => $"{Application.dataPath}/{logFilePath}";

        // Methods

        /// <summary>
        /// Setup static instance prefab in <see cref="Generation"/>
        /// </summary>
        private void Awake() {
            InstancePrefab = instancePrefab;
            NetworkLayout = networkLayout;

            Gates = FindObjectsOfType<Gate.Gate>().OrderBy(g => g.transform.position.y).ToArray();
        }

        /// <summary>
        /// Create first generation on start
        /// </summary>
        private IEnumerator Start() {
            // Skip first 10 frames due to editor lag when starting play mode
            for (var i = 0; i < 10; i++) yield return new WaitForEndOfFrame();

            File.Create(FilePath);
            CurrentGeneration = Generation.Random(generationLifetime, instancesCount, mutationProbability, mutationFactor);
            UpdateGenerationTime();
        }

        /// <summary>
        /// Clean generation and reproduce when generation lifetime exceeded
        /// </summary>
        private void Update() {
            if (GenerationCurrentLifeTime < generationLifetime) return;

            var evaluation = CurrentGeneration.Evaluate();

            LogData(evaluation, CurrentGeneration.Number);
            var oldGeneration = CurrentGeneration;
            CurrentGeneration = Generation.Reproduce(generationLifetime, instancesCount, mutationProbability, mutationFactor, evaluation,
                newRandomCount, oldGeneration.Number + 1);
            Destroy(oldGeneration.gameObject);
            UpdateGenerationTime();
        }

        /// <summary>
        /// Updates time of creation of last generation
        /// </summary>
        private void UpdateGenerationTime() {
            GenerationCreationTime = Time.time;
        }

        private void OnValidate() {
            Time.timeScale = simulationSpeed;
            if (CurrentGeneration != null) CurrentGeneration.Lifetime = generationLifetime;
        }

        private void LogData(GenerationEvaluation evaluation, int generationNumber) {
            var max = evaluation.FitnessMaximum;
            var min = evaluation.FitnessMinimum;
            var avg = evaluation.FitnessAverage;
            var med = evaluation.FitnessMedian;

            using (var sw = File.AppendText(FilePath))
                sw.WriteLine($"{generationNumber}: {max}, {min}, {avg}, {med}");
        }
    }
}