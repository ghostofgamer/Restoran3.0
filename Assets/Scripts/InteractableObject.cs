using Interfaces;
using UnityEngine;

public class InteractableObject : MonoBehaviour,IInteractable
{
    [SerializeField] private Outline _outline;
    
    public void EnableOutline()
    {
        _outline.enabled = true;
    }

    public void DisableOutline()
    {
        _outline.enabled = false;
    }

    public void Action()
    {
        Debug.Log("Interactable to mey");
    }
}