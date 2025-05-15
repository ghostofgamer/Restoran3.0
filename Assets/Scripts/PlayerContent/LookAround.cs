using InputContent;
using SettingsContent;
using UnityEngine;

namespace PlayerContent
{
    public class LookAround : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Transform _playerBody;
        [SerializeField] private float _lookSpeed;
        [SerializeField] private SensitivitySettings _sensitivitySettings; 
        [SerializeField] private float _smoothTime = 0.1f;
        
        private float _verticalLookLimit = 80f;
        private float _rotationX = 0;
        private float _currentRotationX;
        private float _currentRotationY;
        private float _rotationXVelocity;
        private float _rotationYVelocity;
        
        public float LookSpeed => _lookSpeed;

        public void Looking(float x, float y)
        {
            float sensitivity = _sensitivitySettings.SensitivityMouse / 100f; // Нормализация значения чувствительности
            float effectiveLookSpeed = _lookSpeed * sensitivity;

            // Плавное изменение значений вращения
            _rotationX -= y * effectiveLookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -_verticalLookLimit, _verticalLookLimit);
            _currentRotationX = Mathf.SmoothDamp(_currentRotationX, _rotationX, ref _rotationXVelocity, _smoothTime);
            float rotationY = x * effectiveLookSpeed;
            _currentRotationY = Mathf.SmoothDamp(_currentRotationY, rotationY, ref _rotationYVelocity, _smoothTime);

            transform.localRotation = Quaternion.Euler(_currentRotationX, 0, 0);
            _playerBody.Rotate(Vector3.up * _currentRotationY);
            
            
            /*float sensitivity = _sensitivitySettings.SensitivityMouse / 100f; // Нормализация значения чувствительности
            float effectiveLookSpeed = _lookSpeed * sensitivity;

            _rotationX -= y * effectiveLookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -_verticalLookLimit, _verticalLookLimit);
            float rotationY = x * effectiveLookSpeed;
            transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            _playerBody.Rotate(Vector3.up * rotationY);*/
            
            
            
            
            /*_rotationX -= y;
            _rotationX = Mathf.Clamp(_rotationX, -_verticalLookLimit, _verticalLookLimit);
            float rotationY = x;
            transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            _playerBody.Rotate(Vector3.up * rotationY);*/
        }
    }
}