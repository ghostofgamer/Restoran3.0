using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIRotationAndScale : MonoBehaviour
    {
        [SerializeField] private bool _isRight;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Image _targetImage;
        [SerializeField] private float _minScale = 0.5f;
        [SerializeField] private float _scaleSpeed = 1f;

        private void Update()
        {
            {
                float direction = _isRight ? 1f : -1f;
                _targetImage.rectTransform.Rotate(0, 0, direction * _rotationSpeed * Time.deltaTime);
                float t = (Mathf.Sin(Time.time * _scaleSpeed) + 1f) * 0.5f; // Нормализация синусоиды от 0 до 1
                float scale = Mathf.Lerp(_minScale, 1f, t); // Интерполяция от _minScale до 1
                _targetImage.rectTransform.localScale = new Vector3(scale, scale, 1f);
            }
        }
    }
}