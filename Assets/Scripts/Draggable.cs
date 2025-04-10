using InteractableContent;
using Interfaces;
using PlayerContent;
using UnityEngine;

public class Draggable : MonoBehaviour, IDraggable
{
    [SerializeField] private Transform _parentObject;
    [SerializeField] private InteractableObject _interactableObject;

    private void OnEnable()
    {
        _interactableObject.OnAction += Drag;
    }

    private void OnDisable()
    {
        _interactableObject.OnAction -= Drag;
    }

    public virtual void Drag(PlayerInteraction playerInteraction)
    {
        if (playerInteraction.CurrentDraggable != null)
        {
            Debug.Log("Return");
        }
        else
        {
            Debug.Log("DRAG");
            // transform.position = playerInteraction.DraggablePosition.position;
            _parentObject.position = playerInteraction.DraggablePosition.position;
            // transform.rotation = playerInteraction.DraggablePosition.rotation;
            _parentObject.rotation = playerInteraction.DraggablePosition.rotation;
            playerInteraction.SetDraggableObject(this);
        }
    }
}