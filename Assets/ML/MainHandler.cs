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

        private NetworkLayout oldLayout;
        private bool generateRandom;

        [SerializeField] private GenerationParameters generationParameters;
        [Range(0, 20)] public float simulationSpeed;


        private const string dataFilePath = "Logging/data.log";
        private const string fitnessesFilePath = "Logging/fitnesses.log";

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

            ClearLogFiles();

            CurrentGeneration = Generation.Random(generationParameters);

            UpdateGenerationTime();
        }

        /// <summary>
        /// Clean generation and reproduce when generation lifetime exceeded
        /// </summary>
        private void Update() {
            if (GenerationCurrentLifeTime < generationParameters.lifetime) return;

            if (generateRandom) {
                Destroy(CurrentGeneration.GameObject);

                CurrentGeneration = Generation.Random(generationParameters);

                UpdateGenerationTime();

                generateRandom = false;
                return;
            }

            var evaluation = CurrentGeneration.Evaluate();

            LogData(evaluation, CurrentGeneration.Number);

            var oldGeneration = CurrentGeneration;

            CurrentGeneration = Generation.Reproduce(generationParameters, evaluation, oldGeneration.Number + 1);

            foreach (var instance in CurrentGeneration.Instances) instance.fitnessFunction = generationParameters.fitnessFunction;

            Destroy(oldGeneration.GameObject);
            UpdateGenerationTime();
        }

        /// <summary>
        /// Clears log files from old info
        /// </summary>
        private static void ClearLogFiles() {
            File.Create($"{Application.dataPath}/{dataFilePath}");
            File.Create($"{Application.dataPath}/{fitnessesFilePath}");
        }

        /// <summary>
        /// Updates time of creation of last generation
        /// </summary>
        private void UpdateGenerationTime() {
            GenerationCreationTime = Time.time;
        }

        private void OnValidate() {
            Time.timeScale = simulationSpeed;
            
            if (networkLayout != oldLayout) {
                generateRandom = true;
                oldLayout = networkLayout;
            }
        }

        private void LogData(GenerationEvaluation evaluation, int generationNumber) {
            var max = evaluation.FitnessMaximum;
            var min = evaluation.FitnessMinimum;
            var avg = evaluation.FitnessAverage;
            var med = evaluation.FitnessMedian;

            using (var sw = File.AppendText($"{Application.dataPath}/{dataFilePath}"))
                sw.WriteLine($"{generationNumber}: {max}, {min}, {avg}, {med}");

            using (var sw = File.AppendText($"{Application.dataPath}/{fitnessesFilePath}"))
                sw.WriteLine($"{generationNumber}: {string.Join(", ", evaluation.Fitnesses)}");
        }
    }
}