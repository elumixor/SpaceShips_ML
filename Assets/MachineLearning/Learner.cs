using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MachineLearning.Generators;
using MachineLearning.NN;
using MachineLearning.NN.Common;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace MachineLearning {
    public class Learner : MonoBehaviour {
        // Current position (2), rotation (1), scale (2), gate position (2), gate xpos (1), gate width (1)
        public const int InputCount = 2 + 1 + 2 + 2 + 1 + 1;

        // Movement speed (1), rotation speed (1)
        public const int OutputCount = 2;

        public NeuralNetwork nn;
        private List<Gate.Gate> gates;

        private Gate.Gate closestGate;

        private void Start() {
            gates = FindObjectsOfType<Gate.Gate>().OrderBy(g => g.transform.position.y).ToList();
            closestGate = gates[0];
        }

        [SerializeField] private TextMeshPro text;
        private float fitness;

        private float gatesCount = 1;

        private float gateFitness;
        private float speedFitness;

        private bool collided;
        public float Fitness => (fitness + gateFitness) * (collided ? .5f : 1f) * gatesCount;

        private Vector InputVector {
            get {
                var tr = transform;

                var position = tr.position;

                var rotation = tr.localEulerAngles.z;
                var scale = tr.localScale;

                var gatePosition = closestGate.transform.position;
                var gateXPos = closestGate.GatePosition;
                var gateWidth = closestGate.GateWidth;

                return new Vector(
                    position.x, position.y,
                    rotation,
                    scale.x, scale.y,
                    gatePosition.x, gatePosition.y,
                    gateXPos,
                    gateWidth);
            }
        }

        private Rigidbody2D rb;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            lastPosition = transform.position;
        }


        private int updates = 0;

        private Vector3 lastPosition;

        public void FixedUpdate() {
            var newPos = transform.position;

//            var diff = (lastPosition - newPos).magnitude;
//
//            if (lastPosition.y <= newPos.y)
//                speedFitness += diff;

            lastPosition = newPos;

            updates++;

            var iv = InputVector;

            var outputs = nn.Apply(iv);

            var newClosest = gates.First(g => g.transform.position.y > transform.position.y);

            if (newClosest != closestGate) {
                gates.RemoveAt(0);

                gatesCount++;
//                Fitness += 1000;

                closestGate = newClosest;
            }

            var position = transform.position;
            gateFitness = 1f/ Mathf.Pow(1f + Mathf.Abs(newClosest.GatePosition * newClosest.transform.localScale.x * .5f * newClosest.GateWidth - position.x), 2);

//            Fitness -= distance * Time.deltaTime * 10;


            fitness = Mathf.Max(fitness, Mathf.Max(0, position.y));

            text.text = Fitness.ToString(CultureInfo.InvariantCulture);

            var deltaTime = Time.fixedDeltaTime;
            
            var ms = Mathf.Clamp(outputs.values[0], -1, 1) * deltaTime;
            var rot = Mathf.Clamp(outputs.values[1], -360, 360);

//            gameObject.name = $"{updates} updates. ms: {ms}. rot: {rot}";


            var transform1 = transform;
            rb.MovePosition(transform1.position + ms * transform1.up);

            transform.Rotate(0, 0, rot);
//            Quaternion rot = Quaternion.Euler(0, 0, inputHorizontal * oscilation);
            rb.MoveRotation(Quaternion.Euler(0, 0, rot));
        }


        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
                collided = true;
                foreach (var r in GetComponentsInChildren<Renderer>()) r.material.color = Color.red;

//                gameObject.SetActive(false);
            }
        }
    }
}