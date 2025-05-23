using System.Collections;
using InputContent;
using PlayerContent;
using UnityEngine;

namespace TutorialContent
{
    public class PlayerRotator : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private float _rotationSpeed = 5.0f;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private LookAround _lookAround;
        [SerializeField] private GameObject _rotator;

        private Coroutine _rotationCoroutine;
        private Coroutine _lookCoroutine;
        private Quaternion _savedRotation;

        public void RotateToTarget(Transform target)
        {
            _lookAround.LookAtPosition(target.position);
            
            
            /*if (_rotationCoroutine != null)
                StopCoroutine(_rotationCoroutine);

            _rotationCoroutine = StartCoroutine(RotateToTargetCoroutine(target));*/
        }

        private IEnumerator RotateToTargetCoroutine(Transform target)
        {
            Vector3 direction = target.position - _rotator.transform.position;

            SetValue(false);

            Quaternion startRotation = _player.rotation;
            Quaternion endRotation = CalculateTargetRotation(target);

            Quaternion startRotatorRotation = _rotator.transform.rotation;
            Quaternion endRotatorRotation = Quaternion.LookRotation(target.position - _rotator.transform.position);

            float t = 0f;

            while (t < 1.0f)
            {
                t += Time.deltaTime * _rotationSpeed;

                _player.rotation = Quaternion.Lerp(startRotation, endRotation, t);
                _rotator.transform.rotation = Quaternion.Lerp(startRotatorRotation, endRotatorRotation, t);

                yield return null;
            }

            SetValue(true);
        }

        /*private IEnumerator RotateToTargetCoroutine(Transform target)
        {
            Vector3 direction = target.position - _rotator.transform.position;


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
        }*/

        private Quaternion CalculateTargetRotation(Transform target)
        {
            Vector3 direction = target.position - _player.position;
            direction.y = 0;

            if (direction != Vector3.zero)
                return Quaternion.LookRotation(direction);

            return _player.rotation;
        }

        public void SetValue(bool value)
        {
            /*if (value)
            {
                float currentRotationX = _rotator.transform.localRotation.eulerAngles.x;
                Debug.Log("currentRotationX " + currentRotationX);
                _lookAround.SetRotationX(currentRotationX);
            }*/

            _lookAround.enabled = value;
            _playerMovement.enabled = value;
            _playerInput.enabled = value;
        }
    }
}