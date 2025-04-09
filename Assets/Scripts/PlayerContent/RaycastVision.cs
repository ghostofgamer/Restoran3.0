using Interfaces;
using UnityEngine;

namespace PlayerContent
{
    public class RaycastVision : MonoBehaviour
    {
        [SerializeField]private  LayerMask _interactableLayer;
        [SerializeField] private float _maxDistance;
        
        private IInteractable _currentInteractable;
        private GameObject _currentInteractableObject;
        
        private void FixedUpdate()
        {
            CheckOutline();
        }

        private void CheckOutline()
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, _maxDistance, _interactableLayer))
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
                        _currentInteractableObject = hit.collider.gameObject;
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
                _currentInteractableObject = null;
            }
        }
    }
}