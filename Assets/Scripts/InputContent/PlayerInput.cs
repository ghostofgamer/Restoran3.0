using PlayerContent;
using UnityEngine;

namespace InputContent
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private Joystick _joystick;

        private void Update()
        {
            _playerMovement.Move(_joystick.Horizontal,_joystick.Vertical);
        }
    }
}