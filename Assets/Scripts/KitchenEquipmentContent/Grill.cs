using System.Collections;
using System.Linq;
using InteractableContent;
using PlayerContent;
using UnityEngine;

namespace KitchenEquipmentContent
{
    public class Grill : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private Item[] _rawCutletItems;
        [SerializeField] private Item[] _readyCutletItems;
        [SerializeField] private ItemType _currentType;
        [SerializeField] private Animator _animator;
        [SerializeField] private BoxCollider _boxCollider;

        private void OnEnable()
        {
            _interactableObject.OnAction += Action;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= Action;
        }

        public void Action(PlayerInteraction playerInteraction)
        {
            ItemType itemType = playerInteraction.PlayerTray.CurrentType;
            Item[] item = GetItemsByType(itemType);

            if (_currentType == ItemType.Cutlet)
            {
                if (playerInteraction.PlayerTray.CurrentType == ItemType.Cutlet)
                {
                    int activePos = playerInteraction.PlayerTray.GetActivePositionValue(ItemType.Cutlet);
                    int activeCount = CountNotActiveItems(_readyCutletItems);
                    int itemsToPlace = Mathf.Min(activeCount, activePos);

                    if (itemsToPlace > 0)
                    {
                        playerInteraction.PlayerTray.PutAway(ItemType.Cutlet, itemsToPlace);
                        // DeactivateItems(_readyCutletItems, itemsToPlace);
                        ActivateItems(_readyCutletItems, itemsToPlace);
                    }
                }

                if (playerInteraction.PlayerTray.CurrentType == ItemType.Empty)
                {
                    int emptyPos = playerInteraction.PlayerTray.GetEmptyPositionValue(ItemType.Cutlet);
                    int activeCount = CountActiveItems(_readyCutletItems);
                    int itemsToPlace = Mathf.Min(activeCount, emptyPos);

                    if (itemsToPlace > 0)
                    {
                        playerInteraction.PlayerTray.Put(ItemType.Cutlet, itemsToPlace);
                        DeactivateItems(_readyCutletItems, itemsToPlace);
                    }
                }
            }
            else if (_currentType == ItemType.RawCutlet)
            {
                if (playerInteraction.PlayerTray.CurrentType == ItemType.RawCutlet)
                {
                    int emptyPositions =
                        playerInteraction.PlayerTray.GetActivePositionValue(playerInteraction.PlayerTray.CurrentType);
                    int inactiveCount = CountNotActiveItems(item);
                    int itemsToPlace = Mathf.Min(inactiveCount, emptyPositions);

                    Debug.Log("СТОЛ " + itemsToPlace);
                    Debug.Log("emptyPositions " + emptyPositions);
                    Debug.Log("activeCount " + inactiveCount);

                    if (itemsToPlace > 0)
                    {
                        playerInteraction.PlayerTray.PutAway(ItemType.RawCutlet, itemsToPlace);
                        ActivateItems(item, itemsToPlace);
                    }
                }
                else if (playerInteraction.PlayerTray.CurrentType == ItemType.Empty)
                {
                    FryCutlets();
                    
                    /*int activeCount = CountActiveItems(_rawCutletItems);

                    foreach (var rawItem in _rawCutletItems)
                        rawItem.gameObject.SetActive(false);

                    for (int i = 0; i < activeCount; i++)
                        _readyCutletItems[i].gameObject.SetActive(true);

                    _currentType = ItemType.Cutlet;*/
                }
                else
                {
                    Debug.Log("Нельзя положить готовые котлеты, когда на гриле сырые.");
                }
            }
            else
            {
                if (playerInteraction.PlayerTray.CurrentType == ItemType.RawCutlet ||
                    playerInteraction.PlayerTray.CurrentType == ItemType.Cutlet)
                {
                    int inactiveCount = CountNotActiveItems(item);
                    Debug.Log("Гриль колличество не активных " + inactiveCount);
                    int emptyPositions =
                        playerInteraction.PlayerTray.GetActivePositionValue(playerInteraction.PlayerTray.CurrentType);
                    Debug.Log("TRA колличество сырых " + emptyPositions);
                    int itemsToPlace = Mathf.Min(inactiveCount, emptyPositions);
                    Debug.Log("СКОЛЬКО " + itemsToPlace);

                    if (itemsToPlace > 0)
                    {
                        _currentType = playerInteraction.PlayerTray.CurrentType;
                        playerInteraction.PlayerTray.PutAway(playerInteraction.PlayerTray.CurrentType, itemsToPlace);
                        ActivateItems(item, itemsToPlace);
                    }
                }
                else
                {
                    Debug.Log("Нельзя положить готовые котлеты на пустой гриль.");
                }
            }
        }

        private Item[] GetItemsByType(ItemType itemType)
        {
            return itemType switch
            {
                ItemType.RawCutlet => _rawCutletItems,
                ItemType.Cutlet => _readyCutletItems,
                _ => null
            };
        }

        private void ActivateItems(Item[] items, int value)
        {
            if (items == null)
            {
                Debug.LogError("Items array is null.");
                return;
            }

            int activatedCount = 0;

            foreach (var item in items)
            {
                if (!item.gameObject.activeSelf)
                {
                    item.gameObject.SetActive(true);
                    activatedCount++;

                    if (activatedCount >= value)
                        break;
                }
            }
        }

        private void DeactivateItems(Item[] items, int value)
        {
            if (items == null)
            {
                Debug.LogError("Items array is null.");
                return;
            }

            int deactivatedCount = 0;

            for (int i = items.Length - 1; i >= 0; i--)
            {
                if (items[i].gameObject.activeSelf)
                {
                    items[i].gameObject.SetActive(false);
                    deactivatedCount++;

                    if (deactivatedCount >= value)
                        break;
                }
            }

            if (items.All(item => !item.gameObject.activeSelf))
                _currentType = ItemType.Empty;
        }

        private int CountActiveItems(Item[] items)
        {
            int count = 0;
            foreach (var item in items)
            {
                if (item.gameObject.activeSelf)
                {
                    count++;
                }
            }

            return count;
        }

        private int CountNotActiveItems(Item[] items)
        {
            int count = 0;

            foreach (var item in items)
            {
                if (!item.gameObject.activeSelf)
                {
                    count++;
                }
            }

            return count;
        }

        private void FryCutlets()
        {
            StartCoroutine(StartFryCutlets());
        }        
        
        private IEnumerator StartFryCutlets()
        {
            _animator.SetBool("FryCutlet",true);
            _boxCollider.enabled = false;
            yield return new WaitForSeconds(1.5f);
            
            _animator.SetBool("FryCutlet",false);
            
            int activeCount = CountActiveItems(_rawCutletItems);

            foreach (var rawItem in _rawCutletItems)
                rawItem.gameObject.SetActive(false);

            for (int i = 0; i < activeCount; i++)
                _readyCutletItems[i].gameObject.SetActive(true);

            _currentType = ItemType.Cutlet;
            _boxCollider.enabled = true;
        }
    }
}