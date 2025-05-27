using UnityEngine;
using UnityEngine.UI;

namespace DisableInterContent
{
    public class DisableInterViewer : MonoBehaviour
    {
        [SerializeField] private DisablerInter _disablerInter;
        [SerializeField] private Image _fillImage;

        private void OnEnable()
        {
            _disablerInter.CurrentValueChanged += UpdateUI;
        }

        private void OnDisable()
        {
            _disablerInter.CurrentValueChanged -= UpdateUI;
        }

        private void UpdateUI(int currentValue)
        {
            _fillImage.fillAmount = currentValue switch
            {
                0 => 0f,
                1 => 0.5f,
                2 => 1f,
                3 => 1f,
                _ => _fillImage.fillAmount
            };
        }
    }
}