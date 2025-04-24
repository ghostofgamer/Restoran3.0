using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using RestaurantContent;
using RestaurantContent.TrayContent;
using SoContent.AssemblyBurger;
using UnityEngine;

namespace KitchenEquipmentContent
{
    public class SodaAssemblyTable : AssemblyDrinkTable
    {
        [SerializeField] private GameObject[] _emptyCups;
        [SerializeField] private BurgerIngridientSpawner _burgerIngridientSpawner;

        [SerializeField] private AssemblyBurgerItemConfig _assemblyBurgerItemConfig;

        // [SerializeField] private CoffeeCounter _coffeeCounter;
        [SerializeField] private List<Transform> _wellPositions;
        [SerializeField] private Restaurant _restaurant;

        private Coroutine _coroutine;
        private bool _isWorking = false;

        public void PourSoda(ItemType itemType, int index)
        {
            Debug.Log("НАЛИВАЕМ лимонад " + itemType);

            if (_isWorking || ItemContainer.GetActiveItemsValue() <= 0)
                return;

            Debug.Log("КОФЕ");
            _isWorking = true;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Pour(itemType, index));
        }

        private IEnumerator Pour(ItemType itemType, int index)
        {
            Transform availablePosition = _wellPositions.FirstOrDefault(position => position.childCount == 0);

            if (availablePosition != null)
            {
                ItemContainer.DeactivateItems(1);
                _emptyCups[index].SetActive(true);
                yield return new WaitForSeconds(1f);
                _emptyCups[index].SetActive(false);
                Item coffeeInstance = _burgerIngridientSpawner.SpawnItem(itemType);
                coffeeInstance.SetParenContainer(_burgerIngridientSpawner.transform);
                coffeeInstance.gameObject.SetActive(true);
                coffeeInstance.transform.position = _emptyCups[index].transform.position;
                coffeeInstance.transform.rotation = Quaternion.identity;
                coffeeInstance.transform.localScale = _assemblyBurgerItemConfig.GetScale(itemType);

                Sequence sequence = DOTween.Sequence();

                if (_restaurant.TryGetTrayDrinkOrder(ItemType.Coffee, out Tray tray))
                {
                    _restaurant.SetDrinkOrder(tray, coffeeInstance);

                    Debug.Log("TRUE");
                    Transform position = tray.GetFirstAvailablePosition();

                    sequence.Append(coffeeInstance.transform.DOScale(1.15f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(coffeeInstance.transform.DOScale(1.0f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(coffeeInstance.transform.DOMove(position.position, 1f)
                        .SetEase(Ease.InOutQuad));

                    coffeeInstance.transform.SetParent(position);

                    sequence.Join(coffeeInstance.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear))
                        .OnComplete(() => tray.TryCompletedOrder());
                }
                else
                {
                    Debug.Log("FALSE");

                    sequence.Append(coffeeInstance.transform.DOScale(1.15f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(coffeeInstance.transform.DOScale(1.0f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(coffeeInstance.transform.DOMove(availablePosition.position, 0.5f)
                        .SetEase(Ease.InOutQuad));

                    coffeeInstance.transform.SetParent(availablePosition);

                    sequence.Join(coffeeInstance.transform
                        .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                        .SetEase(Ease.Linear));
                    // .OnComplete(() => _coffeeCounter.AddCoffee(coffeeInstance));
                }
            }
            else
            {
                Debug.Log("нету пустых позиций");
            }

            yield return new WaitForSeconds(1f);
            _isWorking = false;
        }
    }
}