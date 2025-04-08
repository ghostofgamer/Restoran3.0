using UnityEngine;

namespace PlayerContent
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;

        private float _gravity = -9.81f;
        private Vector3 _moveDirection;
        private CharacterController _controller;
        private float _verticalVelocity;
        private float _rotationX = 0;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        public void MovePlayer(float horizontal,float vertical)
        {
            _moveDirection = new Vector3(horizontal, 0, vertical);
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= _moveSpeed;

            if (_controller.isGrounded && _verticalVelocity < 0)
                _verticalVelocity = -2f;
            else
                _verticalVelocity += _gravity * Time.deltaTime;

            _moveDirection.y = _verticalVelocity;
            _controller.Move(_moveDirection * Time.deltaTime);
        }
    }
}