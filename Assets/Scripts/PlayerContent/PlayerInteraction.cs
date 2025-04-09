using InputContent;
using Interfaces;
using UnityEngine;

namespace PlayerContent
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Transform _draggablePosition;
        [SerializeField] private GameObject _throwButton;

        private IInteractable _currentInteractable;
        private Vector3 _originalScale;
        private PlayerInput _playerInput;

        public Draggable CurrentDraggable { get; private set; }

        public Transform DraggablePosition => _draggablePosition;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            _playerInput.ActionEvent += Action;
            _playerInput.ThrowEvent += ThrowItem;
        }

        private void OnDisable()
        {
            _playerInput.ActionEvent -= Action;
            _playerInput.ThrowEvent -= ThrowItem;
        }

        public void SetCurrentInteractableObject(IInteractable iInteractable)
        {
            _currentInteractable = iInteractable;
        }

        public void Action()
        {
            if (_currentInteractable != null)
                _currentInteractable.Action(this);
        }

        public void SetDraggableObject(Draggable draggable)
        {
            CurrentDraggable = draggable;
            draggable.transform.SetParent(_draggablePosition);
            draggable.GetComponent<Rigidbody>().isKinematic = true;
            _throwButton.SetActive(true);
        }

        public void ThrowItem()
        {
            if (CurrentDraggable == null)
                return;
            
            CurrentDraggable.GetComponent<Rigidbody>().isKinematic = false;
            CurrentDraggable.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 16f, ForceMode.Impulse);
            ClearDraggableObject();
            _throwButton.SetActive(false);
        }

        private void ClearDraggableObject()
        {
            CurrentDraggable.transform.SetParent(null);
            CurrentDraggable = null;
        }
    }
}