using Interfaces;
using UnityEngine;

namespace PlayerContent
{
    public class RaycastVision : MonoBehaviour
    {
        public LayerMask interactableLayer;
        public float maxDistance = 10f;
        private IInteractable _currentInteractable;

        private void FixedUpdate()
        {
            CheckOutline();
        }

        private void CheckOutline()
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                
                if (interactable != null)
                {
                    if (_currentInteractable != interactable)
                    {
                        if (_currentInteractable != null)
                        {
                            _currentInteractable.DisableOutline();
                        }
                        _currentInteractable = interactable;
                        _currentInteractable.EnableOutline();
                    }
                }
                else
                {
                    DisableCurrentOutline();
                }
            }
            else
            {
                DisableCurrentOutline();
            }
        }
        
       private  void DisableCurrentOutline()
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.DisableOutline();
                _currentInteractable = null;
            }
        }
    }
}