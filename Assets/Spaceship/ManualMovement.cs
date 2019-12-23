using UnityEngine;

namespace Player.Movement {
    public class ManualMovement : MonoBehaviour {
        [SerializeField] private float ms;
        [SerializeField] private float rs;
        
        
        private void FixedUpdate() {
            var tr = transform;

            var deltaTime = Time.fixedDeltaTime;

            if (Input.GetKey(KeyCode.W)) tr.Translate(deltaTime * ms * Vector3.up);
            if (Input.GetKey(KeyCode.S)) tr.Translate(deltaTime * ms * Vector3.down);
            if (Input.GetKey(KeyCode.D)) tr.Rotate(0, 0, -rs * deltaTime);
            if (Input.GetKey(KeyCode.A)) tr.Rotate(0, 0, rs * deltaTime);
        }
    }
}