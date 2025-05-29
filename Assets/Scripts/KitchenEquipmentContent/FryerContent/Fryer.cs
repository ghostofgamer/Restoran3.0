using Enums;
using InteractableContent;
using PlayerContent;
using UI.Screens;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class Fryer : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private GameObject[] _friersContentTables;
        [SerializeField] private EquipmentUIProduct _equipmentUIProduct;
        [SerializeField] private FryerPacking _fryerPacking;
        
        private void OnEnable()
        {
            _interactableObject.OnAction += Action;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= Action;
        }
        
        private void Start()
        {
            foreach (var friersContentTable in _friersContentTables)
                friersContentTable.SetActive(_equipmentUIProduct.IsBuyed());
        }

        public void Action(PlayerInteraction playerInteraction)
        {
            if (playerInteraction.CurrentDraggable != null)
            {
                Debug.Log("в РУКАХ ЧТОТО ЕСТЬ ");
                _fryerPacking.Packing(playerInteraction.CurrentDraggable.GetComponent<ItemBasket>());
            }
            else
            {
                Debug.Log("в РУКАХ ПУСТО ");
            }
        }
    }
}