using InteractableContent;
using Interfaces;
using PlayerContent;
using UnityEngine;

public class Draggable : MonoBehaviour, IDraggable
{
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
            return;
        }
        else
        {
            Debug.Log("DRAG");
            transform.position = playerInteraction.DraggablePosition.position;
            transform.rotation = playerInteraction.DraggablePosition.rotation;
            playerInteraction.SetDraggableObject(this);
        }
    }
}