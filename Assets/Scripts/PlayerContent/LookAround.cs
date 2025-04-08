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

        public float LookSpeed => _lookSpeed;

        public void Looking(float x, float y)
        {
            _rotationX -= y;
            _rotationX = Mathf.Clamp(_rotationX, -_verticalLookLimit, _verticalLookLimit);
            float rotationY = x;
            transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            _playerBody.Rotate(Vector3.up * rotationY);
        }
    }
}