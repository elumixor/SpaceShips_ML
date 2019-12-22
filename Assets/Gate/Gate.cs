using UnityEngine;

namespace Gate {
    [ExecuteInEditMode]
    [SelectionBase]
    public class Gate : MonoBehaviour {
        [SerializeField, Range(0, 1)] private float gateWidth;
        [SerializeField, Range(-1, 1)] private float gatePosition;

        // ReSharper disable once ConvertToAutoPropertyWhenPossible
        public float GateWidth => gateWidth;
        
        // ReSharper disable once ConvertToAutoPropertyWhenPossible
        public float GatePosition => gatePosition;

        [SerializeField] private Transform left;
        [SerializeField] private Transform right;

        private void Update() {
            var leftWidth = (gatePosition + 1) * .5f * (1f - gateWidth);
            var rightWidth = 1f - leftWidth - gateWidth;

            left.localScale = new Vector3(leftWidth, 1, 0);
            right.localScale = new Vector3(rightWidth, 1, 0);

            left.localPosition = new Vector3((leftWidth - 1) * .5f, 0, 0);
            right.localPosition = new Vector3(.5f - rightWidth * .5f, 0, 0);
        }
    }
}