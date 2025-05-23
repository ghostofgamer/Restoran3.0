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
        
        private Coroutine _rotationCoroutine;
        
        public void RotateToTarget(Transform target)
        {
            if (_rotationCoroutine != null)
                StopCoroutine(_rotationCoroutine); 

            _rotationCoroutine = StartCoroutine(RotateToTargetCoroutine(target));
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
    }
}