using InputContent;
using UnityEngine;

namespace PlayerContent
{
    public class LookAround : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;

        public Transform playerBody;
        public float lookSpeed = 2f;

        private float _verticalLookLimit = 80f;
        private float rotationX = 0;
        private bool isRotating = false;

        public void Looking(float x, float y)
        {
            rotationX -= y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -_verticalLookLimit, _verticalLookLimit);
            float rotationY = x * lookSpeed;
            transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            playerBody.Rotate(Vector3.up * rotationY);
        }
    }
}