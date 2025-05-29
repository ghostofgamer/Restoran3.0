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