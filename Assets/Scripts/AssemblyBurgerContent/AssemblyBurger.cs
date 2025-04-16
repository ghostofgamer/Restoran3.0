using System.Collections.Generic;
using System.Linq;
using Enums;
using InteractableContent;
using SoContent.AssemblyBurger;
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

        // private Stack<Item> _ingredientStack = new Stack<Item>();

        private Stack<(Item item, ItemContainer container)> _ingredientStack = new Stack<(Item, ItemContainer)>();

        private Camera _camera;
        private int _maxIngredients = 15;
        private ItemContainer _lastItemContainer;
        private int _lastIndexBun = -1;

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
                                Debug.Log("selectedContainer.CurrentItemsType[0] " +
                                          selectedContainer.CurrentItemsType[0]);
                                HandleContainerSelection(selectedContainer.CurrentItemsType[0], selectedContainer,
                                    0, true);
                            }
                            else
                            {
                                Debug.Log("selectedContainer.CurrentItemsType[1] " +
                                          selectedContainer.CurrentItemsType[1]);
                                HandleContainerSelection(selectedContainer.CurrentItemsType[1], selectedContainer,
                                    1, true);
                            }

                            /*for (int i = 0; i < activeItems.Length; i++)
                            {
                                Debug.Log("Active count in sub-array " + i + ": " + activeItems[i]);
                            }*/
                        }
                        else
                        {
                            int activeItems = selectedContainer.GetActiveItemsValue();
                            Debug.Log("activeItems: " + activeItems);
                            Debug.Log("selectedContainer: " + selectedContainer.name);
                            Debug.Log("ItemType: " + selectedContainer.CurrentItemContainer);

                            if (activeItems > 0)
                            {
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
                        ItemType burgerType = GetMatchingRecipe();

                        if (burgerType != ItemType.Empty)
                        {
                            CreateBurger(burgerType);
                            Debug.Log("Бургер " + burgerType);
                        }
                        else
                        {
                            Debug.Log("Не правильная сборка Бургер ");
                        }
                    }

                    BurgerBoard selectedBoard = hit.collider.GetComponent<BurgerBoard>();
                    Sauce sauce = hit.collider.GetComponent<Sauce>();

                    if (sauce != null)
                    {
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
            Vector3 position = _burgerBoard.CenterPosition.position;
            Quaternion rotation = _burgerBoard.CenterPosition.rotation;
            Vector3 scale = _assemblyBurgerItemConfig.GetScale(type);

            if (_ingredientStack.Count > 0)
            {
                Item previousItem = _ingredientStack.Peek().item;
                AssemblyIngredient assemblyIngredient = previousItem.GetComponent<AssemblyIngredient>();

                if (assemblyIngredient != null)
                {
                    if (assemblyIngredient.PositionUpIngredient != null)
                        position = assemblyIngredient.PositionUpIngredient.position;
                    else
                        return;
                }
            }

            Item item = _burgerIngridientSpawner.SpawnItem(type);

            _lastItemContainer = itemContainer;

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
            }

            item.transform.position = position;
            item.transform.rotation = rotation;
            item.transform.localScale = scale;

            /*item.transform.position = _burgerBoard.CenterPosition.position;
            item.transform.rotation = _burgerBoard.CenterPosition.rotation;
            item.transform.localScale = _assemblyBurgerItemConfig.GetScale(type);*/
            item.gameObject.SetActive(true);

            _ingredientStack.Push((item, itemContainer));
        }

        public void UndoLastSelection()
        {
            if (_ingredientStack.Count > 0)
            {
                var (lastItem, container) = _ingredientStack.Pop();
                lastItem.transform.position = Vector3.zero;
                lastItem.gameObject.SetActive(false);

                // _lastItemContainer.ActivateItems(1);
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
            }
            else
            {
                Debug.Log("No ingredients to undo.");
            }
        }

        private ItemType GetMatchingRecipe()
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
        }

        private void CreateBurger(ItemType burgerType)
        {
            GameObject burgerPrefab =
                _burgerPrefabPairs.FirstOrDefault(pair => pair.BurgerType == burgerType)?.BurgerPrefab;

            if (burgerPrefab != null)
            {
                Transform availablePosition = _burgerPositions.FirstOrDefault(position => position.childCount == 0);

                if (availablePosition != null)
                {
                    GameObject burgerInstance =
                        Instantiate(burgerPrefab, availablePosition.position, Quaternion.identity);

                    burgerInstance.transform.SetParent(availablePosition);
                    // burgerInstance.transform.position = Vector3.zero;
                    Debug.Log("Бургер создан: " + burgerType);

                    while (_ingredientStack.Count > 0)
                    {
                        var (lastItem, container) = _ingredientStack.Pop();
                        lastItem.gameObject.SetActive(false);
                        // Destroy(lastItem.gameObject);
                    }
                }
                else
                {
                    Debug.Log(
                        "Нет места, если хотите создать новый, освободите  место или сделайте бургер из нынешнего заказа");
                }
            }
            else
            {
                Debug.LogError("Префаб для бургера типа " + burgerType + " не найден.");
            }
        }
    }
}