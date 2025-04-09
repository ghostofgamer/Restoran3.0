using Interfaces;
using UnityEngine;

namespace PlayerContent
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Transform _draggablePosition;

        private IInteractable _currentInteractable;
        private Vector3 _originalScale;

        public Draggable CurrentDraggable { get; private set; }

        public Transform DraggablePosition => _draggablePosition;

        private Vector3 originalScale;
        private Vector3 originalPosition;
        private Quaternion originalRotation;


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
            // draggable.transform.parent = _draggablePosition.transform;
            draggable.transform.SetParent(_draggablePosition);
            draggable.GetComponent<Rigidbody>().isKinematic = true;
        }

        public void ClearDraggableObject()
        {
            CurrentDraggable.transform.SetParent(null);
            CurrentDraggable = null;
        }

        public void ThrowItem()
        {
            if (CurrentDraggable == null)
                return;
            
            CurrentDraggable.GetComponent<Rigidbody>().isKinematic = false;
            CurrentDraggable.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 10f, ForceMode.Impulse);
            ClearDraggableObject();
        }
    }
}