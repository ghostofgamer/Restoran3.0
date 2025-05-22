using System;
using Enums;
using Interfaces;
using PlayerContent;
using TutorialContent;
using UnityEngine;

public class ActionButtonActivator : MonoBehaviour
{
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private GameObject _touchActionAnim;

    private void OnEnable()
    {
        _playerInteraction.CurrentDraggerChanger += Action;
    }

    private void OnDisable()
    {
        _playerInteraction.CurrentDraggerChanger -= Action;
    }

    private void Start()
    {
        if ((int)_tutorial.CurrentType > (int)TutorialType.TakeBoxBuns)
            Completed();
    }

    public void Completed()
    {
        _touchActionAnim.SetActive(false);
        enabled = false;
    }

    private void Action(IInteractable iInteractable)
    {
        if ((int)_tutorial.CurrentType > (int)TutorialType.TakeBoxBuns)
            return;

        if (iInteractable != null)
        {
            MonoBehaviour monoBehaviour = iInteractable as MonoBehaviour;

            if (monoBehaviour != null)
            {
                GameObject interactableObject = monoBehaviour.gameObject;
                ItemBox itemBasket = interactableObject.GetComponent<ItemBox>();

                if (itemBasket != null)
                {
                    if ((int)_tutorial.CurrentType <= (int)TutorialType.TakeBoxBuns)
                        _touchActionAnim.SetActive(true);
                }
            }
        }
        else
        {
            if ((int)_tutorial.CurrentType <= (int)TutorialType.TakeBoxBuns)
                _touchActionAnim.SetActive(false);
        }
    }
}