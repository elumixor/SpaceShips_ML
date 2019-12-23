using System;
using System.Linq;
using Common;
using ML.NN;
using UnityEngine;

namespace ML {
    public class GenerationInstance : MonoBehaviour {
        // self[position(2), rotation(1), scale(2)], gate[obj.position(2), obj.scale(2), width(1), position(1)]
        public const int InputsCount = 11;

        // forward movement speed (1), rotation (1)
        public const int OutputsCount = 2;

        /// <summary>
        /// Neural network the instance
        /// </summary>
        public NeuralNetwork NN { get; private set; }

        /// <summary>
        /// Fitness of the instance
        /// </summary>
        public float Fitness { get; private set; }

        /// <summary>
        /// Instances' maximum lifetime
        /// </summary>
        public float Lifetime { get; set; }

        /// <summary>
        /// Time when the instance was created
        /// </summary>
        public float CreationTime { get; private set; }

        /// <summary>
        /// Vector of inputs
        /// </summary>
        public InstanceInputs Inputs { get; private set; }

        /// <summary>
        /// Vector of outputs
        /// </summary>
        public InstanceOutputs Outputs { get; private set; }

        /// <summary>
        /// Helper class for inputs
        /// </summary>
        [Serializable]
        public class InstanceInputs {
            public Vector values = new Vector(InputsCount);

            public Vector2 SelfPosition {
                get => new Vector2(values.Values[0], values.Values[1]);
                set {
                    values.Values[0] = value.x;
                    values.Values[1] = value.y;
                }
            }

            public float SelfRotation {
                get => values.Values[2];
                set => values.Values[2] = value;
            }

            public Vector2 SelfScale {
                get => new Vector2(values.Values[3], values.Values[4]);
                set {
                    values.Values[3] = value.x;
                    values.Values[4] = value.y;
                }
            }

            public Vector2 GateObjectPosition {
                get => new Vector2(values.Values[5], values.Values[6]);
                set {
                    values.Values[5] = value.x;
                    values.Values[6] = value.y;
                }
            }

            public Vector2 GateObjectScale {
                get => new Vector2(values.Values[7], values.Values[8]);
                set {
                    values.Values[7] = value.x;
                    values.Values[8] = value.y;
                }
            }

            public float GateWidth {
                get => values.Values[9];
                set => values.Values[9] = value;
            }

            public float GatePosition {
                get => values.Values[10];
                set => values.Values[10] = value;
            }

            /// <summary>
            /// Update inputs with values
            /// </summary>
            public void Update(Transform self, Gate.Gate gate) {
                SelfPosition = self.position;
                SelfRotation = self.localEulerAngles.z;
                SelfScale = self.localScale;

                var gateTransform = gate.transform;

                GateObjectPosition = gateTransform.position;
                GateObjectScale = gateTransform.localScale;
                GateWidth = gate.GateWidth;
                GatePosition = gate.GatePosition;
            }
        }

        /// <summary>
        /// Helper class for outputs
        /// </summary>
        [Serializable]
        public class InstanceOutputs {
            public Vector values;

            public float Movement => values.Values[0];
            public float Rotation => values.Values[1];
        }

        /// <summary>
        /// Rigidbody component
        /// </summary>
        private Rigidbody2D rb;

        /// <summary>
        /// Assign components and generate NN
        /// </summary>
        private void Awake() {
            NN = new NeuralNetwork(MainHandler.NetworkLayout);
            CreationTime = Time.time;
            Inputs = new InstanceInputs();
            Outputs = new InstanceOutputs();
            rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Updates inputs, feeds them into NN and applies them to transform and rigidbody
        /// </summary>
        private void FixedUpdate() {
            if (Time.time >= CreationTime + Lifetime) {
                Deactivate();
                return;
            }

            var tr = transform;
            var position = tr.position;
            
            var gate = MainHandler.Gates.First(g => g.transform.position.y > transform.position.y);
            
            Inputs.Update(tr, gate);
            Outputs.values = NN.Apply(Inputs.values);

            var ms = Time.fixedDeltaTime * Mathf.Clamp(Outputs.Movement, -1, 1);
            var rot = Time.fixedDeltaTime * Mathf.Clamp(Outputs.Rotation, -1, 1) * 100;

            rb.MovePosition(position + ms * tr.up);
            rb.MoveRotation(Quaternion.Euler(0,0, rot));
            
            tr.Rotate(0,0, rot);
            
            Fitness = position.y;
        }

        private void Deactivate() {
            gameObject.SetActive(false);
        }
    }
}