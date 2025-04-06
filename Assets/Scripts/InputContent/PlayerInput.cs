using PlayerContent;
using UnityEngine;

namespace InputContent
{
    public class PlayerInput : MonoBehaviour
    {
        private const string MouseX = "Mouse X";
        private const string MouseY = "Mouse Y";

        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private LookAround _lookAround;
        [SerializeField] private Joystick _joystick;

        private bool _isRotating;

        private void Update()
        {
            if (_isRotating)
                _lookAround.Looking(Input.GetAxis(MouseX), Input.GetAxis(MouseY));

            _playerMovement.Move(_joystick.Horizontal, _joystick.Vertical);
        }

        public void OnPointerDown()=> _isRotating = true;

        public void OnPointerUp()=> _isRotating = false;
    }
}