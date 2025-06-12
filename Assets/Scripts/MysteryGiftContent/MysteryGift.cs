using System;
using InteractableContent;
using PlayerContent;
using UnityEngine;

namespace MysteryGiftContent
{
    public class MysteryGift : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;

        public event Action BoxActivation;
        public event Action BoxDeactivation;
        
        private void OnEnable()
        {
            _interactableObject.OnAction += Action;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= Action;
        }

        public void DeactivateBox()
        {
            BoxDeactivation?.Invoke();
            gameObject.SetActive(false); 
        }

        private void Action(PlayerInteraction playerInteraction)
        {
            Debug.Log("Активирую мистический бокс");
            BoxActivation?.Invoke();
        }
    }
}