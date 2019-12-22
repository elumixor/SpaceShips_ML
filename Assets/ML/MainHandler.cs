using System;
using System.Linq;
using ML.NN;
using UnityEditor.UIElements;
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
        
        [SerializeField, Range(0, 10)] private float generationLifetime;
        [SerializeField, Range(0, 1)] private float mutationProbability;
        [SerializeField, Range(0, 1)] private float mutationFactor;

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
        private void Start() {
            CurrentGeneration = Generation.Random(generationLifetime, instancesCount, mutationProbability, mutationFactor);
            UpdateGenerationTime();
        }

        /// <summary>
        /// Clean generation and reproduce when generation lifetime exceeded
        /// </summary>
        private void Update() {
            if (GenerationCurrentLifeTime < generationLifetime) return;

            var oldGeneration = CurrentGeneration;
            CurrentGeneration = Generation.Reproduce(oldGeneration);
            Destroy(oldGeneration);
            UpdateGenerationTime();
        }

        /// <summary>
        /// Updates time of creation of last generation
        /// </summary>
        private void UpdateGenerationTime() {
            GenerationCreationTime = Time.time;
        }
    }
}