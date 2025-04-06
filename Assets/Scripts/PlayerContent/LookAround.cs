using InputContent;
using UnityEngine;

namespace PlayerContent
{
    public class LookAround : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Transform _playerBody;
        [SerializeField] private float _lookSpeed;

        private float _verticalLookLimit = 80f;
        private float _rotationX = 0;

        public void Looking(float x, float y)
        {
            _rotationX -= y * _lookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -_verticalLookLimit, _verticalLookLimit);
            float rotationY = x * _lookSpeed;
            transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            _playerBody.Rotate(Vector3.up * rotationY);
        }
    }
}