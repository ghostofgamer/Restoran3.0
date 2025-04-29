using System.Collections.Generic;
using DG.Tweening;
using InteractableContent;
using ItemContent;
using PlayerContent;
using UnityEngine;

namespace ShelfContent
{
    public class Shelf : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private Transform[] _positions;
        [SerializeField] private List<ItemBasket> _itemBaskets = new List<ItemBasket>();
        [SerializeField] private List<ItemDrinkPackage> _itemDrinkPackages = new List<ItemDrinkPackage>();

        private void OnEnable()
        {
            _interactableObject.OnAction += Action;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= Action;
        }

        private void Action(PlayerInteraction playerInteraction)
        {
            if (playerInteraction.CurrentDraggable != null)
            {
                int freePosCount = GetFreePositionCount();
                Debug.Log(freePosCount);

                if (freePosCount <= 0)
                    return;

                ItemBasket basket = playerInteraction.CurrentDraggable.GetComponent<ItemBasket>();
                Draggable draggable = playerInteraction.CurrentDraggable.GetComponent<Draggable>();
                ItemDrinkPackage drinkPackage = playerInteraction.CurrentDraggable.GetComponent<ItemDrinkPackage>();

                if (draggable != null)
                {
                    draggable.PutOnShelf();
                    
                    Transform position = GetFreePosition();
                    
                    Sequence sequence = DOTween.Sequence();
                    
                    if (basket != null)
                    {
                        _itemBaskets.Add(basket);

                        basket.SetShelf(this);

                        playerInteraction.ClearDraggableObject();
                        basket.transform.SetParent(position);

                        sequence.Append(basket.transform.DOMove(position.position, 0.3f)
                            .SetEase(Ease.InOutQuad));
                        sequence.Join(basket.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear));
                    }

                    if (drinkPackage != null)
                    {
                        _itemDrinkPackages.Add(drinkPackage);
                        drinkPackage.SetShelf(this);
                        playerInteraction.ClearDraggableObject();
                        drinkPackage.transform.SetParent(position);
                        
                        
                        sequence.Append(drinkPackage.transform.DOMove(position.position, 0.3f)
                            .SetEase(Ease.InOutQuad));
                        sequence.Join(drinkPackage.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear));
                    }
                }
                else
                {
                    Debug.Log("NULL");
                }
            }
        }

        public int GetFreePositionCount()
        {
            int freeCount = 0;

            foreach (var position in _positions)
            {
                if (position.childCount == 0)
                    freeCount++;
            }

            return freeCount;
        }

        public Transform GetFreePosition()
        {
            foreach (var position in _positions)
            {
                if (position.childCount == 0)
                    return position;
            }

            return null;
        }

        public void Remove(ItemBasket itemBasket)
        {
            _itemBaskets.Remove(itemBasket);
        }
        
        public void RemoveDrinkPackage(ItemDrinkPackage drinkPackage)
        {
            _itemDrinkPackages.Remove(drinkPackage);
        }
    }
}