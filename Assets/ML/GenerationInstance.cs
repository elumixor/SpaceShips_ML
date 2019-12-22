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
        public class InstanceInputs {
            public Vector Values = new Vector(InputsCount);

            public Vector2 SelfPosition {
                get => new Vector2(Values.Values[0], Values.Values[1]);
                set {
                    Values.Values[0] = value.x;
                    Values.Values[1] = value.y;
                }
            }

            public float SelfRotation {
                get => Values.Values[2];
                set => Values.Values[2] = value;
            }

            public Vector2 SelfScale {
                get => new Vector2(Values.Values[3], Values.Values[4]);
                set {
                    Values.Values[3] = value.x;
                    Values.Values[4] = value.y;
                }
            }

            public Vector2 GateObjectPosition {
                get => new Vector2(Values.Values[5], Values.Values[6]);
                set {
                    Values.Values[5] = value.x;
                    Values.Values[6] = value.y;
                }
            }

            public Vector2 GateObjectScale {
                get => new Vector2(Values.Values[7], Values.Values[8]);
                set {
                    Values.Values[7] = value.x;
                    Values.Values[8] = value.y;
                }
            }

            public float GateWidth {
                get => Values.Values[9];
                set => Values.Values[9] = value;
            }

            public float GatePosition {
                get => Values.Values[10];
                set => Values.Values[10] = value;
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
        public class InstanceOutputs {
            public Vector Values;

            public float Movement => Values.Values[0];
            public float Rotation => Values.Values[1];
        }
        
        private void Awake() {
            NN = new NeuralNetwork(MainHandler.NetworkLayout);
            CreationTime = Time.time;
            Inputs = new InstanceInputs();
            Outputs = new InstanceOutputs();
            
        }

        private void Update() {
            if (Time.time >= CreationTime + Lifetime) Deactivate();

            var gate = MainHandler.Gates.First(g => g.transform.position.y > transform.position.y);
            
            Inputs.Update(transform, gate);
            Outputs.Values = NN.Apply(Inputs.Values);
        }

        private void Deactivate() {
            gameObject.SetActive(false);
        }
    }
}