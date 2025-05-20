using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using InteractableContent;
using PlayerContent.LevelContent;
using RestaurantContent;
using RestaurantContent.TrayContent;
using SettingsContent.SoundContent;
using SoContent.AssemblyBurger;
using TutorialContent;
using UnityEngine;

namespace AssemblyBurgerContent
{
    public class AssemblyBurger : MonoBehaviour
    {
        [SerializeField] private BurgerBoard _burgerBoard;
        [SerializeField] private BurgerIngridientSpawner _burgerIngridientSpawner;
        [SerializeField] private AssemblyBurgerItemConfig _assemblyBurgerItemConfig;
        [SerializeField] private BurgerRecipeConfig _burgerRecipeConfig;
        [SerializeField] private List<BurgerPrefabPair> _burgerPrefabPairs;
        [SerializeField] private List<Transform> _burgerPositions;
        [SerializeField] private Restaurant _restaurant;
        [SerializeField] private BurgersCounter _burgersCounter;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private TutorialAssemblyBurger _tutorialAssemblyBurger;

        // private Stack<Item> _ingredientStack = new Stack<Item>();

        private Stack<(Item item, ItemContainer container)> _ingredientStack = new Stack<(Item, ItemContainer)>();

        private Camera _camera;
        private int _maxIngredients = 15;
        private ItemContainer _lastItemContainer;
        private int _lastIndexBun = -1;
        private bool _isAnimationInProgress = false;

        public event Action StackChanged;

        public Stack<(Item item, ItemContainer container)> IngredientStack => _ingredientStack;

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
                    Debug.Log("HIT " + hit.collider.name);

                    ItemContainer selectedContainer = hit.collider.GetComponent<ItemContainer>();

                    if (selectedContainer != null &&
                        selectedContainer.CurrentItemContainer != ItemType.PackageBurgerPaper)
                    {
                        if (_ingredientStack.Count > _maxIngredients)
                            return;

                        if (selectedContainer.IsAdditionalItemsContainer)
                        {
                            int[] activeItems = selectedContainer.GetActivePositions();

                            if (_ingredientStack.Count > 0)
                            {
                                if (activeItems[0] <= 0)
                                {
                                    Debug.Log("булочек не хватает");
                                    return;
                                }

                                if (_tutorial.CurrentType == TutorialType.LetsMakeFirstBurger)
                                {
                                    _tutorialAssemblyBurger.NextItemPackages();
                                }

                                Debug.Log("selectedContainer.CurrentItemsType[0] " +
                                          selectedContainer.CurrentItemsType[0]);
                                HandleContainerSelection(selectedContainer.CurrentItemsType[0], selectedContainer,
                                    0, true);
                            }
                            else
                            {
                                if (activeItems[1] <= 0)
                                {
                                    Debug.Log("булочек не хватает");
                                    return;
                                }

                                if (_tutorial.CurrentType == TutorialType.LetsMakeFirstBurger)
                                {
                                    _tutorialAssemblyBurger.NextItemCutlet();
                                }

                                Debug.Log("selectedContainer.CurrentItemsType[1] " +
                                          selectedContainer.CurrentItemsType[1]);
                                HandleContainerSelection(selectedContainer.CurrentItemsType[1], selectedContainer,
                                    1, true);
                            }
                        }
                        else
                        {
                            int activeItems = selectedContainer.GetActiveItemsValue();
                            Debug.Log("activeItems: " + activeItems);
                            Debug.Log("selectedContainer: " + selectedContainer.name);
                            Debug.Log("ItemType: " + selectedContainer.CurrentItemContainer);

                            if (activeItems > 0)
                            {
                                if (_tutorial.CurrentType == TutorialType.LetsMakeFirstBurger)
                                {
                                    if (selectedContainer.CurrentItemContainer == ItemType.Cutlet)
                                        _tutorialAssemblyBurger.NextItemKetchup();
                                }

                                HandleContainerSelection(selectedContainer.CurrentItemContainer, selectedContainer);
                            }
                            else
                            {
                                Debug.Log("Нету ингридиентов этого типа " + selectedContainer.CurrentItemContainer);
                            }
                        }
                    }
                    else if (selectedContainer != null &&
                             selectedContainer.CurrentItemContainer == ItemType.PackageBurgerPaper)
                    {
                        Debug.Log("пробуем собрать бургер по рецепту");

                        int activePackageBurgerPaper = selectedContainer.GetActiveItemsValue();

                        if (activePackageBurgerPaper <= 0)
                        {
                            Debug.Log("упаковок нету уже");
                        }
                        else
                        {
                            Debug.Log("упаковок осталось " + activePackageBurgerPaper);

                            ItemType burgerType = GetMatchingRecipe();

                            if (burgerType != ItemType.Empty)
                            {
                                CreateBurger(burgerType, selectedContainer);
                                Debug.Log("Бургер " + burgerType);
                                // selectedContainer.DeactivateItems(1);
                            }
                            else
                            {
                                Debug.Log("Не правильная сборка Бургер ");
                            }
                        }
                    }

                    BurgerBoard selectedBoard = hit.collider.GetComponent<BurgerBoard>();
                    Sauce sauce = hit.collider.GetComponent<Sauce>();

                    if (sauce != null)
                    {
                        if (_tutorial.CurrentType == TutorialType.LetsMakeFirstBurger)
                        {
                            if (sauce.ItemType == ItemType.Ketchup)
                                _tutorialAssemblyBurger.NextItemBunTop();
                        }

                        HandleContainerSelection(sauce.ItemType);
                    }

                    if (selectedBoard != null)
                    {
                        UndoLastSelection();
                    }
                }
            }
        }

        private void HandleContainerSelection(ItemType type, ItemContainer itemContainer = null,
            int index = -1,
            bool isAdditional = false)
        {
            if (_isAnimationInProgress)
            {
                return;
            }

            Vector3 position = _burgerBoard.CenterPosition.position;
            Quaternion rotation = _burgerBoard.CenterPosition.rotation;
            Vector3 scale = _assemblyBurgerItemConfig.GetScale(type);

            if (_ingredientStack.Count > 0)
            {
                Item previousItem = _ingredientStack.Peek().item;
                AssemblyIngredient assemblyIngredient = previousItem.GetComponent<AssemblyIngredient>();

                // StackChanged?.Invoke();

                if (assemblyIngredient != null)
                {
                    if (assemblyIngredient.PositionUpIngredient != null)
                        position = assemblyIngredient.PositionUpIngredient.position;
                    else
                        return;
                }
            }

            Item item = _burgerIngridientSpawner.SpawnItem(type);

            SoundPlayer.Instance.PlayPutTray();

            _lastItemContainer = itemContainer;
            item.gameObject.SetActive(true);

            if (itemContainer != null)
            {
                if (!isAdditional)
                {
                    itemContainer.DeactivateItems(1);
                }
                else
                {
                    itemContainer.DeactivateItems(1, index);
                    _lastIndexBun = index;
                }

                Debug.Log("НУУУ");

                Vector3 initialPosition = itemContainer.transform.position;
                initialPosition.y += 0.3f;

                item.transform.position = initialPosition;
                // item.gameObject.SetActive(true);
                item.transform.localScale = scale;

                _isAnimationInProgress = true;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(item.transform.DOMove(position, 0.3f).SetEase(Ease.InOutQuad));
                sequence.Join(item.transform.DORotate(rotation.eulerAngles, 0.3f).SetEase(Ease.InOutQuad));
                sequence.OnComplete(() =>
                {
                    // Сбрасываем флаг после завершения анимации
                    _isAnimationInProgress = false;
                });
            }
            else
            {
                item.transform.position = position;
                item.transform.rotation = rotation;
                item.transform.localScale = scale;
            }

            _ingredientStack.Push((item, itemContainer));
            Debug.Log("_ingredient Stack Count " + _ingredientStack.Count);
            StackChanged?.Invoke();
        }

        public void UndoLastSelection()
        {
            if ((int)_tutorial.CurrentType < (int)TutorialType.LetsSetPrice)
                return;

            if (_isAnimationInProgress)
            {
                return;
            }

            if (_ingredientStack.Count > 0)
            {
                var (lastItem, container) = _ingredientStack.Pop();
                Debug.Log("_ingredient Stack Count " + _ingredientStack.Count);
                StackChanged?.Invoke();

                if (container != null)
                {
                    _isAnimationInProgress = true;
                    Sequence sequence = DOTween.Sequence();

                    SoundPlayer.Instance.PlayPutTray();

                    sequence.Append(lastItem.transform.DOMove(container.transform.position, 0.3f)
                        .SetEase(Ease.InOutQuad));
                    sequence.Join(lastItem.transform.DORotate(container.transform.eulerAngles, 0.3f)
                        .SetEase(Ease.InOutQuad));
                    sequence.OnComplete(() =>
                    {
                        _isAnimationInProgress = false;
                        lastItem.transform.position = Vector3.zero;
                        lastItem.gameObject.SetActive(false);

                        if (container != null)
                        {
                            switch (lastItem.ItemType)
                            {
                                case ItemType.BunTop:
                                    container.ActivateItems(1, 0);
                                    break;
                                case ItemType.BunLow:
                                    container.ActivateItems(1, 1);
                                    break;
                                default:
                                    container.ActivateItems(1);
                                    break;
                            }
                        }
                    });
                }
                else
                {
                    lastItem.transform.position = Vector3.zero;
                    lastItem.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.Log("No ingredients to undo.");
            }
        }

        /*private ItemType GetMatchingRecipe()
        {
            List<ItemType> itemTypes = _ingredientStack.Select(tuple => tuple.item.ItemType).ToList();

            foreach (var recipe in _burgerRecipeConfig.recipes)
            {
                if (recipe.ItemTypes.SequenceEqual(itemTypes))
                {
                    return recipe.BurgerType;
                }
            }

            return ItemType.Empty;
        }*/

        private ItemType GetMatchingRecipe()
        {
            List<ItemType> itemTypes = _ingredientStack.Select(tuple => tuple.item.ItemType).ToList();

            foreach (var recipe in _burgerRecipeConfig.recipes)
            {
                // Получаем список типов ингредиентов рецепта
                List<ItemType> recipeItemTypes = new List<ItemType>(recipe.ItemTypes);

                if (recipeItemTypes.Count > 0)
                {
                    // Исключаем последний ингредиент из рецепта
                    ItemType excludedItemType = recipeItemTypes.First();
                    recipeItemTypes.Remove(excludedItemType);

                    // Выводим исключенный ингредиент в консоль
                    Debug.Log($"Excluded ingredient from recipe {recipe.BurgerType}: {excludedItemType}");
                }

                if (recipeItemTypes.SequenceEqual(itemTypes))
                {
                    return recipe.BurgerType;
                }
            }

            return ItemType.Empty;
        }

        private void CreateBurger(ItemType burgerType, ItemContainer containerPackageBurger)
        {
            GameObject burgerPrefab =
                _burgerPrefabPairs.FirstOrDefault(pair => pair.BurgerType == burgerType)?.BurgerPrefab;

            if (burgerPrefab != null)
            {
                Transform availablePosition = _burgerPositions.FirstOrDefault(position => position.childCount == 0);

                if (availablePosition != null)
                {
                    /*GameObject burgerInstance =
                        Instantiate(burgerPrefab, availablePosition.position, Quaternion.identity);*/

                    Item burgerInstance = _burgerIngridientSpawner.SpawnItem(burgerType);
                    burgerInstance.SetParenContainer(_burgerIngridientSpawner.transform);
                    burgerInstance.gameObject.SetActive(true);
                    burgerInstance.transform.position = _burgerBoard.CenterPosition.position;
                    burgerInstance.transform.rotation = Quaternion.identity;

                    if(_tutorial.CurrentType==TutorialType.LetsMakeFirstBurger)
                    {
                        _tutorialAssemblyBurger.CompletedAssemblyBurger();
                        _tutorial.SetCurrentTutorialStage(TutorialType.LetsMakeFirstBurger);
                    }
                    
                    containerPackageBurger.DeactivateItems(1);

                    _playerLevel.AddExp(5);

                    /*GameObject burgerInstance =
                        Instantiate(burgerPrefab, _burgerBoard.CenterPosition.position, Quaternion.identity);*/

                    Sequence sequence = DOTween.Sequence();

                    if (_restaurant.TryGetTrayOrder(burgerType, out Tray tray))
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
                    }

                    Debug.Log("Бургер создан: " + burgerType);

                    while (_ingredientStack.Count > 0)
                    {
                        var (lastItem, container) = _ingredientStack.Pop();
                        Debug.Log("_ingredient Stack Count " + _ingredientStack.Count);
                        lastItem.gameObject.SetActive(false);
                    }

                    StackChanged?.Invoke();
                }
                else
                {
                    if (_restaurant.TryGetTrayOrder(burgerType, out Tray tray))
                    {
                        Item burgerInstance = _burgerIngridientSpawner.SpawnItem(burgerType);
                        burgerInstance.SetParenContainer(_burgerIngridientSpawner.transform);
                        burgerInstance.gameObject.SetActive(true);
                        burgerInstance.transform.position = _burgerBoard.CenterPosition.position;
                        burgerInstance.transform.rotation = Quaternion.identity;

                        containerPackageBurger.DeactivateItems(1);

                        _restaurant.SetBurgerOrder(tray, burgerInstance);
                        _playerLevel.AddExp(5);

                        Sequence sequence = DOTween.Sequence();
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

                        while (_ingredientStack.Count > 0)
                        {
                            var (lastItem, container) = _ingredientStack.Pop();
                            Debug.Log("_ingredient Stack Count " + _ingredientStack.Count);
                            lastItem.gameObject.SetActive(false);

                            // Destroy(lastItem.gameObject);
                        }

                        StackChanged?.Invoke();
                    }

                    else
                    {
                        Debug.Log(
                            "Нет места, если хотите создать новый, освободите  место или сделайте бургер из нынешнего заказа");
                    }
                }
            }
            else
            {
                Debug.LogError("Префаб для бургера типа " + burgerType + " не найден.");
            }
        }

        public void SimpleCreateStartBurgers(ItemType burgerType)
        {
            Transform availablePosition = _burgerPositions.FirstOrDefault(position => position.childCount == 0);
            Item burgerInstance = _burgerIngridientSpawner.SpawnItem(burgerType);
            burgerInstance.gameObject.SetActive(true);
            burgerInstance.transform.SetParent(availablePosition);
            burgerInstance.transform.position = availablePosition.position;
            burgerInstance.transform.localRotation = Quaternion.Euler(0, 0, 0);
            burgerInstance.transform.localScale = Vector3.one;
            _burgersCounter.AddBurger(burgerInstance);
        }
    }
}