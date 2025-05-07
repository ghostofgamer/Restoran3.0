using InteractableContent;
using PlayerContent;
using UnityEngine;

namespace GarbageContent
{
    public class GarbagePackage : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;

        private void OnEnable()
        {
            _interactableObject.OnAction += Action;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= Action;
        }

        private void Action(PlayerInteraction playerInteraction)
        {
            
        }
    }
}