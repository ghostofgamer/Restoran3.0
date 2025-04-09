using System;
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

    public void Drag(PlayerInteraction playerInteraction)
    {
        Debug.Log("DRAG");
    }
}