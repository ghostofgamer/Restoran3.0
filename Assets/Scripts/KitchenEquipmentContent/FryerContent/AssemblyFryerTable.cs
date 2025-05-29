using Enums;
using InteractableContent;
using PlayerContent;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class AssemblyFryerTable : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private FryerContainer[] _fryerContainers;
        [SerializeField] private FryerFrying _fryerFrying;
        [SerializeField] private FryerTool[] _fryerTools;
        [SerializeField] private ItemContainer[] _itemContainersPackage;

        private void OnEnable()
        {
            _interactableObject.OnAction += Action;
            _fryerFrying.FryCompleted += FillTable;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= Action;
            _fryerFrying.FryCompleted -= FillTable;
        }

        private void Action(PlayerInteraction playerInteraction)
        {
            Debug.Log("сборочный стол фритюрницы");

            if (playerInteraction.CurrentDraggable != null)
            {
                Debug.Log("В руках есть");

                ItemBasket itemBasket = playerInteraction.CurrentDraggable.GetComponent<ItemBasket>();

                if (itemBasket != null)
                {
                    foreach (var itemContainer in _itemContainersPackage)
                    {
                        if (itemBasket.ItemType == itemContainer.CurrentItemContainer)
                        {
                            Debug.Log("Совпадение контенера и коробки по типу " + itemBasket.ItemType);

                           int emptyPosContainer =  itemContainer.GetEmptyPosition();
                           int activeItemBasket = itemBasket.GetActiveValueItems();

                           if (emptyPosContainer > 0 && activeItemBasket > 0)
                           {
                               int itemsToPlace = Mathf.Min(emptyPosContainer, activeItemBasket);
                               
                               itemBasket.TransferProduct(itemsToPlace, itemContainer.Positions);
                               itemContainer.ActivateItems(itemsToPlace);
                           }
                        }
                    }
                }
            }
        }

        private void FillTable()
        {
            Debug.Log("Пробуем пополнить стол ингриидиентами");

            foreach (var fryerTool in _fryerTools)
            {
                if (!fryerTool.IsRaw)
                {
                    int valueFryerTool = fryerTool.GetCountActiveWellItems();
                    
                    if (valueFryerTool > 0)
                    {
                        FryerContainer fryerContainer = GetCompatibleFryerContainer(fryerTool.ItemType);

                        if (fryerContainer != null)
                        {
                            int emptyContainerPosition = fryerContainer.GetInactiveValue();
                            Debug.Log("Пустых мест в контейнере " + emptyContainerPosition);
                            int itemsToPlace = Mathf.Min(emptyContainerPosition, valueFryerTool);
                            Debug.Log("Меньшее число  " + itemsToPlace);
                            
                            fryerContainer.ActivateItems(itemsToPlace);
                            fryerTool.DeactivateWellItems(itemsToPlace);
                        }
                    }
                }
            }
        }

        public FryerContainer GetCompatibleFryerContainer(ItemType itemType)
        {
            foreach (var fryerTool in _fryerContainers)
            {
                if (fryerTool.ItemType == itemType)
                    return fryerTool;
            }

            return null;
        }
    }
}