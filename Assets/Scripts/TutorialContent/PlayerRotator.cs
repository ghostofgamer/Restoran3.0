using System.Collections;
using InputContent;
using PlayerContent;
using UnityEngine;

namespace TutorialContent
{
    public class PlayerRotator : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private  float _rotationSpeed = 5.0f;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private LookAround _lookAround;
        [SerializeField] private GameObject _rotator;
        
        private Coroutine _rotationCoroutine;
        private Coroutine _lookCoroutine;
        
        public void RotateToTarget(Transform target)
        {
           
            
            if (_lookCoroutine != null)
            {
                StopCoroutine(_lookCoroutine);
            }

            _lookCoroutine = StartCoroutine(LookAtTarget(target));

            /*if (_rotationCoroutine != null)
                StopCoroutine(_rotationCoroutine);

            _rotationCoroutine = StartCoroutine(RotateToTargetCoroutine(target));*/
        }
        
        private IEnumerator RotateToTargetCoroutine(Transform target)
        {
            SetValue(false);
            
            Quaternion startRotation = _player.rotation;
            Quaternion endRotation = CalculateTargetRotation(target);

            float t = 0f;
            
            while (t < 1.0f)
            {
                t += Time.deltaTime * _rotationSpeed;
                _player.rotation = Quaternion.Lerp(startRotation, endRotation, t);
                yield return null;
            }
            
            SetValue(true);
        }
        
        private Quaternion CalculateTargetRotation(Transform target)
        {
            Vector3 direction = target.position - _player.position;
            direction.y = 0; 
            
            if (direction != Vector3.zero)
                return Quaternion.LookRotation(direction);
            
            return _player.rotation;
        }

        private void SetValue(bool value)
        {
            _lookAround.enabled = value;
            _playerMovement.enabled = value;
            _playerInput.enabled = value;
        }
        
        private float _currentRotationY;
        private float _currentRotationX;
        [SerializeField] private float _smoothTime = 0.1f;
        
        public void Looking(Transform target)
        {
            Vector3 direction = target.position - _rotator.transform.position;
            direction.y = 0; 

            if (direction != Vector3.zero)
            {
                
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Плавное изменение значений вращения
                transform.localRotation = Quaternion.Euler(_currentRotationX, 0, 0);
                _player.rotation = Quaternion.Slerp(_player.rotation, targetRotation, _smoothTime);
            }
        }
        
        private IEnumerator LookAtTarget(Transform target)
        {
            while (true)
            {
                Vector3 direction = target.position - _rotator.transform.position;
                direction.y = 0;

                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);

                    // Плавное изменение значений вращения
                    transform.localRotation = Quaternion.Euler(_currentRotationX, 0, 0);
                    _player.rotation = Quaternion.Slerp(_player.rotation, targetRotation, _smoothTime);

                    // Проверяем, достиг ли объект целевого вращения
                    if (Quaternion.Angle(_player.rotation, targetRotation) < 0.1f)
                    {
                        break;
                    }
                }

                yield return null;
            }
        }
    }
}