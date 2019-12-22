using System.Globalization;
using TMPro;
using UnityEngine;

namespace Info {
    public class InfoCanvas : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI valueText;

        public void SetValue(float value) {
            transform.position = Vector3.up * value;
            valueText.text = value.ToString(CultureInfo.InvariantCulture);
        }
    }
}