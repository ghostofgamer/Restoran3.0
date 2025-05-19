using Enums;
using InputContent;
using SettingsContent;
using TutorialContent;
using UI.Screens.TutorialScreens;
using UnityEngine;

namespace PlayerContent
{
    public class LookAround : MonoBehaviour
    {
        [SerializeField] private TutorialType _tutorialType;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private LookAroundScreen _lookAroundScreen;
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
            if ((int)_tutorial.CurrentType < (int)_tutorialType)
            {
                Debug.Log("(int)_tutorial.CurrentType < (int)_tutorialType");
                return;
            }

            if ((int)_tutorial.CurrentType == (int)_tutorialType)
            {
                Debug.Log("Выполнил Current этап тутора");
                _lookAroundScreen.CloseScreen();
                _tutorial.SetCurrentTutorialStage(_tutorialType);
            }

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
        }
    }
}