using System.Collections.Generic;
using System.Linq;
using InteractableContent;
using RecipesContent;
using SoContent.AssemblyBurger;
using UnityEngine;

namespace AssemblyBurgerContent
{
    public class AssemblyBurger : MonoBehaviour
    {
        [SerializeField] private BurgerBoard _burgerBoard;
        [SerializeField] private BurgerIngridientSpawner _burgerIngridientSpawner;
        [SerializeField] private AssemblyBurgerItemConfig _assemblyBurgerItemConfig;
        [SerializeField] private List<Recipe> _recipes;
        [SerializeField] private Dictionary<ItemType, GameObject> _burgerPrefabs;
        
        private Stack<Item> _ingredientStack = new Stack<Item>();
        private Camera _camera;
        private int _maxIngredients = 15;

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
                                HandleContainerSelection(activeItems[0], selectedContainer.CurrentItemsType[0]);
                            }
                            else
                            {
                                Debug.Log("selectedContainer.CurrentItemsType[1] " +
                                          selectedContainer.CurrentItemsType[1]);
                                HandleContainerSelection(activeItems[1], selectedContainer.CurrentItemsType[1]);
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

                            HandleContainerSelection(activeItems, selectedContainer.CurrentItemContainer);
                        }
                    }
                    else if (selectedContainer != null &&
                             selectedContainer.CurrentItemContainer == ItemType.PackageBurgerPaper)
                    {
                        Debug.Log("пробуем собрать бургер по рецепту");
                        ItemType burgerType = GetMatchingRecipe();
                        
                        if (burgerType != ItemType.Empty)
                        {
                            // CreateBurger(burgerType);
                            Debug.Log("Бургер " + burgerType);
                        }
                        else
                        {
                            Debug.Log("Не правильная сборка Бургер ");
                        }
                       
                    }

                    BurgerBoard selectedBoard = hit.collider.GetComponent<BurgerBoard>();

                    if (selectedBoard != null)
                    {
                        UndoLastSelection();
                    }
                }
            }
        }

        private void HandleContainerSelection(int value, ItemType type)
        {
            Vector3 position = _burgerBoard.CenterPosition.position;
            Quaternion rotation = _burgerBoard.CenterPosition.rotation;
            Vector3 scale = _assemblyBurgerItemConfig.GetScale(type);

            if (_ingredientStack.Count > 0)
            {
                Item previousItem = _ingredientStack.Peek();
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

            item.transform.position = position;
            item.transform.rotation = rotation;
            item.transform.localScale = scale;

            /*item.transform.position = _burgerBoard.CenterPosition.position;
            item.transform.rotation = _burgerBoard.CenterPosition.rotation;
            item.transform.localScale = _assemblyBurgerItemConfig.GetScale(type);*/
            item.gameObject.SetActive(true);

            _ingredientStack.Push(item);
        }

        public void UndoLastSelection()
        {
            if (_ingredientStack.Count > 0)
            {
                Item lastItem = _ingredientStack.Pop();
                lastItem.transform.position = Vector3.zero;
                lastItem.gameObject.SetActive(false);
                // Destroy(lastItem.gameObject);
            }
            else
            {
                Debug.Log("No ingredients to undo.");
            }
        }
        
        private ItemType GetMatchingRecipe()
        {
            List<ItemType> itemTypes = _ingredientStack.Select(item => item.ItemType).ToList();

            foreach (var recipe in _recipes)
            {
                if (recipe.ItemTypes.SequenceEqual(itemTypes))
                {
                    return recipe.BurgerType;
                }
            }
            
            return ItemType.Empty;
        }
    }
}