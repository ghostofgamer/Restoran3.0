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

        private float _direction;
        private float _t;
        private float _scale;

        private void Update()
        {
            {
                _direction = _isRight ? 1f : -1f;
                _targetImage.rectTransform.Rotate(0, 0, _direction * _rotationSpeed * Time.deltaTime);
                _t = (Mathf.Sin(Time.time * _scaleSpeed) + 1f) * 0.5f;
                _scale = Mathf.Lerp(_minScale, 1f, _t);
                _targetImage.rectTransform.localScale = new Vector3(_scale, _scale, 1f);
            }
        }
    }
}