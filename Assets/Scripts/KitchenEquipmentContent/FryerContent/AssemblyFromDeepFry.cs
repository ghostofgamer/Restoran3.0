using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using InteractableContent;
using PlayerContent.LevelContent;
using UnityEngine;
using UnityEngine.Serialization;

namespace KitchenEquipmentContent.FryerContent
{
    public class AssemblyFromDeepFry : MonoBehaviour
    {
        [SerializeField] private FryerContainer[] _fryerContainers;
        [SerializeField] private Transform[] _itemWellPositions;
        [SerializeField] private Transform _centerPos;
        [SerializeField] private PlayerLevel _playerLevel;

        [FormerlySerializedAs("_burgerPrefabPairs")] [SerializeField]
        private List<ItemPrefabPair> _itemPrefabPairs = new List<ItemPrefabPair>();

        [SerializeField] private BurgerIngridientSpawner _burgerIngridientSpawner;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    // ItemContainer selectedContainer = hit.collider.GetComponent<ItemContainer>();
                    TriggerContainer selectedContainer = hit.collider.GetComponent<TriggerContainer>();

                    if (selectedContainer != null)
                    {
                        Debug.Log("selectedContainer нашелся");

                        ItemType itemType = selectedContainer.ItemType;

                        foreach (var _fryerContainer in _fryerContainers)
                        {
                            if (_fryerContainer.ItemType == itemType)
                                Create(_fryerContainer, selectedContainer.ItemContainer, itemType);
                        }
                    }
                    else
                    {
                        Debug.Log("selectedContainer NULL");
                    }
                }
            }
        }

        private void Create(FryerContainer fryerContainer, ItemContainer itemContainer, ItemType itemType)
        {
            Debug.Log("Пробуем собрать  " + itemType);
            int activeItemContainers = fryerContainer.GetActiveValue();
            Debug.Log("activeItemContainers  " + activeItemContainers);
            if (activeItemContainers <= 0)
                return;
            
            GameObject itemPrefab = _itemPrefabPairs.FirstOrDefault(pair => pair.Type == itemType)?.Prefab;

            if (itemPrefab == null)
                return;
            Debug.Log("itemPrefab  " + itemPrefab);
            
            Transform availablePosition = _itemWellPositions.FirstOrDefault(position => position.childCount == 0);
            Debug.Log("Позиций пустых для готовых " + availablePosition);

            if (availablePosition == null)
                return;
            
            Item itemInstance = _burgerIngridientSpawner.SpawnItem(itemType);
            itemInstance.SetParenContainer(_burgerIngridientSpawner.transform);
            itemInstance.gameObject.SetActive(true);
            itemInstance.transform.position = _centerPos.position;
            itemInstance.transform.rotation = Quaternion.identity;
            
            _playerLevel.AddExp(5);
            
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(itemInstance.transform.DOScale(1.15f, 0.3f).SetEase(Ease.InOutQuad));
            sequence.Append(itemInstance.transform.DOScale(1.0f, 0.3f).SetEase(Ease.InOutQuad));
            sequence.Append(itemInstance.transform.DOMove(availablePosition.position, 0.5f)
                .SetEase(Ease.InOutQuad));

            itemInstance.transform.SetParent(availablePosition);

            sequence.Join(itemInstance.transform
                .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear));
            // .OnComplete(() => _burgersCounter.AddBurger(itemInstance));
        }
    }
}