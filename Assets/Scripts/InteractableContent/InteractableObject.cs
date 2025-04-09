using System;
using Interfaces;
using PlayerContent;
using UnityEngine;

namespace InteractableContent
{
    public class InteractableObject : MonoBehaviour,IInteractable
    {
        [SerializeField] private Outline _outline;
        
        public event Action<PlayerInteraction> OnAction;
    
        public void EnableOutline()
        {
            _outline.enabled = true;
        }

        public void DisableOutline()
        {
            _outline.enabled = false;
        }

        public virtual void Action(PlayerInteraction playerInteraction)
        {
            Debug.Log("Interactable to mey " + gameObject.name);
            OnAction?.Invoke(playerInteraction);
        }
    }
}