using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using SoContent.AssemblyBurger;
using UnityEngine;

namespace KitchenEquipmentContent.AssemblyTables.CoffeeTableContent
{
    public class CoffeeAssemblyTable : AssemblyDrinkTable
    {
        [SerializeField] private GameObject _emptyCup;
        [SerializeField] private BurgerIngridientSpawner _burgerIngridientSpawner;
        [SerializeField] private AssemblyBurgerItemConfig _assemblyBurgerItemConfig;
        [SerializeField] private CoffeeCounter _coffeeCounter;
        [SerializeField] private List<Transform> _wellPositions;

        private Coroutine _coroutine;

        public void PourCoffee()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Pour());
        }

        private IEnumerator Pour()
        {
            Transform availablePosition = _wellPositions.FirstOrDefault(position => position.childCount == 0);

            if (availablePosition != null)
            {
                _emptyCup.SetActive(true);
                yield return new WaitForSeconds(1f);
                _emptyCup.SetActive(false);
                Item coffeeInstance = _burgerIngridientSpawner.SpawnItem(ItemType.Coffee);
                coffeeInstance.SetParenContainer(_burgerIngridientSpawner.transform);
                coffeeInstance.gameObject.SetActive(true);
                coffeeInstance.transform.position = _emptyCup.transform.position;
                coffeeInstance.transform.rotation = Quaternion.identity;
                coffeeInstance.transform.localScale = _assemblyBurgerItemConfig.GetScale(ItemType.Coffee);
                
                Sequence sequence = DOTween.Sequence();
                sequence.Append(coffeeInstance.transform.DOScale(1.15f, 0.3f).SetEase(Ease.InOutQuad));
                sequence.Append(coffeeInstance.transform.DOScale(1.0f, 0.3f).SetEase(Ease.InOutQuad));
                sequence.Append(coffeeInstance.transform.DOMove(availablePosition.position, 0.5f)
                    .SetEase(Ease.InOutQuad));

                coffeeInstance.transform.SetParent(availablePosition);

                sequence.Join(coffeeInstance.transform
                        .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                        .SetEase(Ease.Linear))
                    .OnComplete(() => _coffeeCounter.AddCoffee(coffeeInstance));
                
                
                /*if (_restaurant.TryGetTrayOrder(burgerType, out Tray tray))
                {
                    _restaurant.SetBurgerOrder(tray, burgerInstance);

                    Debug.Log("TRUE");
                    Transform position = tray.GetFirstAvailablePosition();

                    sequence.Append(burgerInstance.transform.DOScale(1.15f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(burgerInstance.transform.DOScale(1.0f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(burgerInstance.transform.DOMove(position.position, 1f)
                        .SetEase(Ease.InOutQuad));

                    burgerInstance.transform.SetParent(position);

                    sequence.Join(burgerInstance.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear))
                        .OnComplete(() => tray.TryCompletedOrder());
                }
                else
                {
                    Debug.Log("FALSE");

                    sequence.Append(burgerInstance.transform.DOScale(1.15f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(burgerInstance.transform.DOScale(1.0f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(burgerInstance.transform.DOMove(availablePosition.position, 0.5f)
                        .SetEase(Ease.InOutQuad));

                    burgerInstance.transform.SetParent(availablePosition);

                    sequence.Join(burgerInstance.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear))
                        .OnComplete(() => _burgersCounter.AddBurger(burgerInstance));
                }*/

            }
            else
            {
                Debug.Log("нету пустых позиций");
            }
        }
    }
}