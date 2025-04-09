using Interfaces;
using UnityEngine;

namespace PlayerContent
{
    public class PlayerInteraction : MonoBehaviour
    {
        private IInteractable _currentInteractable;
        private Draggable _currentDraggable;

        public void SetCurrentInteractableObject(IInteractable iInteractable)
        {
            _currentInteractable = iInteractable;
        }

        public void Action()
        {
            if (_currentInteractable != null)
                _currentInteractable.Action(this);
        }

        public void SetDraggableObject()
        {
            
        }

        public void ClearDraggableObject()
        {
            
        }
    }
}