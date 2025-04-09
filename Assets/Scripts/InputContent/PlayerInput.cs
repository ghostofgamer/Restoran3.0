using PlayerContent;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputContent
{
    public class PlayerInput : MonoBehaviour
    {
        private const string MouseX = "Mouse X";
        private const string MouseY = "Mouse Y";

        [SerializeField] private LookAroundEventTrigger _lookAroundEventTrigger;
        [SerializeField] private ActionTriggerEvent _actionEventTrigger;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private LookAround _lookAround;
        [SerializeField] private Joystick _joystick;
        [SerializeField] private PlayerInteraction _playerInteraction;

        private bool _isRotating;
        private Vector2 _lastPointerPosition;
        private bool _isTouchActive;

        private void Start()
        {
            _lookAroundEventTrigger.InitPointer(OnDown, OnDrag, OnUp);
            _actionEventTrigger.InitPointer(Action);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                _playerInteraction.Action();

            if (!Application.isMobilePlatform)
            {
                HandleMouseInput();
                _playerMovement.MovePlayer(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }

            _playerMovement.MovePlayer(_joystick.Horizontal, _joystick.Vertical);
        }

        private void HandleMouseInput()
        {
            float x = Input.GetAxis(MouseX) * _lookAround.LookSpeed;
            float y = Input.GetAxis(MouseY) * _lookAround.LookSpeed;
            _lookAround.Looking(x, y);
        }

        private void OnDown(PointerEventData eventData)
        {
            if (!_isTouchActive)
            {
                _isRotating = true;
                _lastPointerPosition = eventData.position;
                _isTouchActive = true;
            }
        }

        private void OnUp(PointerEventData eventData)
        {
            if (_isTouchActive)
            {
                _isRotating = false;
                _isTouchActive = false;
            }
        }

        private void OnDrag(PointerEventData eventData)
        {
            if (_isTouchActive)
            {
                if (_isRotating)
                {
                    Vector2 delta = eventData.position - _lastPointerPosition;
                    _lastPointerPosition = eventData.position;
                    float x = delta.x * _lookAround.LookSpeed * Time.deltaTime;
                    float y = delta.y * _lookAround.LookSpeed * Time.deltaTime;
                    _lookAround.Looking(x, y);
                }
            }
        }

        private void Action(PointerEventData eventData)
        {
            _playerInteraction.Action();
        }
    }
}