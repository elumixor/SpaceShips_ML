using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Info;
using MachineLearning.Generators;
using MachineLearning.NN;
using MachineLearning.NN.ActivationFunctions;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace MachineLearning {
    /// <summary>
    /// Generates objects with neural networks attached, based on generator
    /// </summary>
    public class ObjectsGenerator : MonoBehaviour {
        [SerializeField] private NetworkGenerator networkGenerator;
        [SerializeField] private Learner playerPrefab;
        [SerializeField] private InfoCanvas highScore;
        [SerializeField] private InfoCanvas median;

        [SerializeField] private int objectsCount;
        [SerializeField] private int topPlayers;
        [SerializeField] private int newPlayers;

        [SerializeField, Range(1, 100)] private float generationLifetime;
        [SerializeField, Range(0, 10)] private float simulationSpeed;
        [SerializeField, Range(0, 1)] private float deviationValue;
        [SerializeField, Range(0, 1)] private float deviationFrequency;
        [SerializeField, Range(0, 1)] private float positionRandomness;


        private List<(NeuralNetwork nn, float Fitness)> topNN;
        private List<GameObject> currentGeneration;
        private int generation;
        private float generationStartTime;
        private Gate.Gate gate;


        private IEnumerator Start() {
            gate = FindObjectOfType<Gate.Gate>();
            Application.targetFrameRate = 120;

            for (var i = 0; i < 10; i++) {
                yield return new WaitForEndOfFrame();
            }

            NextGeneration();
        }

        private void FixedUpdate() {
            if (!(Time.time - generationStartTime > generationLifetime)) return;

            KillGeneration();
            NextGeneration();
        }

        private void NextGeneration() {
            generation++;

            generationStartTime = Time.time;
            currentGeneration = new List<GameObject>();

            for (var i = 0; i < objectsCount; i++) {
                var instance = Instantiate(playerPrefab,
                    gate.transform.localScale.x * .5f * UnityEngine.Random.Range(-1f, 1f) * positionRandomness *
                    Vector3.right,
                    Quaternion.identity);


                NeuralNetwork nn;
                if (topNN == null) nn = networkGenerator.Generate(i, Learner.InputCount, Learner.OutputCount);
                else {
                    nn = i < topPlayers
                        ? topNN[i].nn
                        : i < topPlayers + newPlayers
                            ? networkGenerator.Generate(i, Learner.InputCount, Learner.OutputCount)
                            : ChildNN;
                }

                instance.nn = nn;
                var go = instance.gameObject;
                go.name = $"G{generation}: {i}";
                currentGeneration.Add(go);
            }
        }

        private NeuralNetwork ChildNN {
            get {
                var totalFitness = topNN.Select(n => n.Fitness).Sum();
                var dict = topNN.Select(n => (fitness: n.Fitness / totalFitness, NN: n.nn)).ToArray();

                var r = UnityEngine.Random.value;
                var i = 0;

                while (i < dict.Length - 1 && r > dict[i].fitness) i++;

                //                Debug.Log(newNN.Same(dict[i].NN) ? "same" : "different");
                return networkGenerator.RandomizeNN(dict[i].NN, deviationValue, deviationFrequency);
            }
        }

        private void KillGeneration() {
            if (currentGeneration == null) return;
            
            var ordered = currentGeneration
                .Select(go => go.GetComponent<Learner>())
                .OrderByDescending(l => l.Fitness).ToArray();

            var ftn = ordered.Select(l => l.Fitness).ToArray();
            var top = ftn[0];

            var count = ftn.Length;
            var m = ftn[count / 2] + ftn[(count - 1) / 2];
            m /= 2;

            highScore.SetValue(top);
            median.SetValue(m);

            topNN = ordered.Take(topPlayers)
                .Select(l => (l.nn, l.Fitness)).ToList();

            foreach (var go in currentGeneration) Destroy(go);
        }

        private void OnValidate() {
            Time.timeScale = simulationSpeed;
        }
    }
}