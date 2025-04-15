using System;
using System.Collections.Generic;
using CameraContent;
using DG.Tweening;
using InteractableContent;
using PlayerContent;
using UnityEngine;

public class AssemblyTable : MonoBehaviour
{
    [SerializeField] private ItemContainer[] _itemContainers;
    [SerializeField] private BurgerIngridientSpawner _burgerIngridientSpawner;
    [SerializeField] private InteractableObject _interactableObject;
    
    [SerializeField] private Transform _cameraCurrentPosition;
    [SerializeField] private CameraPositionChanger _cameraPositionChanger;

    private Dictionary<ItemType, ItemContainer> _containersByItemType;

    public event Action BurgerAssemblyBeginig;

    private void Awake()
    {
        _containersByItemType = new Dictionary<ItemType, ItemContainer>();

        foreach (var container in _itemContainers)
        {
            _containersByItemType[container.CurrentItemContainer] = container;
        }
    }

    private void OnEnable()
    {
        _interactableObject.OnAction += HandlePlayerInteraction;
    }

    private void OnDisable()
    {
        _interactableObject.OnAction -= HandlePlayerInteraction;
    }

    public void HandlePlayerInteraction(PlayerInteraction playerInteraction)
    {
        if (playerInteraction.CurrentDraggable != null)
        {
            ItemBasket basket = playerInteraction.CurrentDraggable.GetComponent<ItemBasket>();

            if (basket != null)
            {
                ItemContainer targetContainer = GetContainerForItemType(basket.ItemType);

                if (targetContainer != null)
                {
                    if (targetContainer.IsAdditionalItemsContainer && basket.IsAdditionalItemsBasket)
                    {
                        int[] emptyPositions = targetContainer.GetEmptyPositions();
                        int[] activeItems = basket.GetActiveValueArrayItems();

                        for (int i = 0; i < emptyPositions.Length; i++)
                        {
                            Debug.Log("emptyPositions " + emptyPositions[i]);
                        }

                        for (int i = 0; i < activeItems.Length; i++)
                        {
                            Debug.Log("activeItems " + activeItems[i]);
                        }

                        if (emptyPositions.Length == activeItems.Length)
                        {
                            Debug.Log("Одинаковое коолличество видов продуктов ");

                            for (int i = 0; i < emptyPositions.Length; i++)
                            {
                                if (emptyPositions[i] > 0 && activeItems[i] > 0)
                                {
                                    int itemsToPlace = Mathf.Min(emptyPositions[i], activeItems[i]);
                                    Debug.Log("itemsToPlace " + itemsToPlace);

                                    basket.RemoveItem(itemsToPlace, i);
                                    basket.TransferProduct(itemsToPlace, i, targetContainer.AdditionalArrayPositions);
                                    targetContainer.ActivateItems(itemsToPlace, i);
                                }
                                else
                                {
                                    Debug.Log(" ЛИБо нету места или активных продуктов");
                                }
                            }
                        }
                    }
                    else
                    {
                        int emptyPosition = targetContainer.GetEmptyPosition();
                        int activeItems = basket.GetActiveValueItems();

                        if (emptyPosition > 0 && activeItems > 0)
                        {
                            int itemsToPlace = Mathf.Min(emptyPosition, activeItems);
                            basket.TransferProduct(itemsToPlace, targetContainer.Positions);
                            targetContainer.ActivateItems(itemsToPlace);
                            Debug.Log($"Placed {itemsToPlace} items in container for {basket.ItemType}");
                        }
                        else
                        {
                            Debug.Log(
                                $"No space in container or no active items in basket. Container empty positions: {emptyPosition}, Basket active items: {activeItems}");
                        }
                    }
                }
                else
                {
                    Debug.LogError($"No container found for item type: {basket.ItemType}");
                }
            }
            else
            {
                Debug.LogError("The draggable object is not an ItemBasket.");
            }
        }
        else if (playerInteraction.PlayerTray.IsActive)
        {
            ItemContainer targetContainer = GetContainerForItemType(playerInteraction.PlayerTray.CurrentType);

            if (targetContainer != null)
            {
                int emptyPosition = targetContainer.GetEmptyPosition();
                int activeItems =
                    playerInteraction.PlayerTray.GetActivePositionValue(playerInteraction.PlayerTray.CurrentType);

                if (emptyPosition > 0 && activeItems > 0)
                {
                    int itemsToPlace = Mathf.Min(emptyPosition, activeItems);

                    // basket.TransferProduct(itemsToPlace, targetContainer.Positions);
                    playerInteraction.PlayerTray.PutAway(playerInteraction.PlayerTray.CurrentType, itemsToPlace);
                    targetContainer.ActivateItems(itemsToPlace);
                }
                else
                {
                    Debug.Log(
                        $"No space in container or no active items in basket. Container empty positions: {emptyPosition}, Basket active items: {activeItems}");
                }
            }
        }
        else
        {
            BurgerAssemblyBeginig?.Invoke();
            _cameraPositionChanger.ChangePosition(_cameraCurrentPosition);
            Debug.Log("No draggable object in player's hands.");
        }
    }

    private ItemContainer GetContainerForItemType(ItemType itemType)
    {
        if (_containersByItemType.TryGetValue(itemType, out var container))
        {
            return container;
        }

        return null;
    }
}