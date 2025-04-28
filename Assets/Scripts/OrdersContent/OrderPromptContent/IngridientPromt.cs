using System.Linq;
using AssemblyBurgerContent;
using Enums;
using IngredientsContent;
using OrdersContent;
using SoContent;
using SoContent.AssemblyBurger;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IngridientPromt : MonoBehaviour
{
    [SerializeField] private AssemblyBurger _assemblyBurger;
    [SerializeField] private Image[] _ingredients;
    [SerializeField] private BurgerRecipeConfig _burgerRecipeConfig;
    [SerializeField] private IngredientsConfig _ingredientsConfig;
    [SerializeField] private ElementSelector _elementSelector;
    [SerializeField] private IngredientsViewer[] _ingredientsViewers;

    private Recipes _recipes;
    private Order _order;

    private void OnEnable()
    {
        _assemblyBurger.StackChanged += CheckIngredientsProgress;
    }

    private void OnDisable()
    {
        _assemblyBurger.StackChanged -= CheckIngredientsProgress;
    }

    public void SetIngredients(Order order)
    {
        _order = order;

        /*
        foreach (var ingredient in _ingredients)
            ingredient.gameObject.SetActive(false);*/

        foreach (var ingredient in _ingredientsViewers)
            ingredient.gameObject.SetActive(false);


        if (order.BurgerItemOrder != ItemType.Empty)
        {
            _recipes = _burgerRecipeConfig.GetRecipeByBurgerType(order.BurgerItemOrder);

            /*for (int i = _recipes.ItemTypes.Count - 1, j = 0; i >= 0; i--, j++)
            {
                _ingredients[j].sprite = _ingredientsConfig.GetSprite(_recipes.ItemTypes[i]);
                _ingredients[j].gameObject.SetActive(true);
            }*/

            for (int i = _recipes.ItemTypes.Count - 1, j = 0; i >= 0; i--, j++)
            {
                _ingredientsViewers[j].SetDefault(_ingredientsConfig.GetSprite(_recipes.ItemTypes[i]));
                _ingredientsViewers[j].gameObject.SetActive(true);
            }
        }
    }

    /*public void CheckIngredientsProgress()
    {
        if (_order != null)
        {
            if (_order.IsBurgerCompleted)
            {
                /*foreach (var ingredient in _ingredients)
                    ingredient.gameObject.SetActive(false);#1#

                foreach (var ingredient in _ingredientsViewers)
                    ingredient.gameObject.SetActive(false);

                if (!_order.IsDrinkCompleted)
                {
                    Debug.Log("_order.DrinkItemOrder " + _order.DrinkItemOrder);
                    Debug.Log("Sprite  " + _ingredientsConfig.GetSprite(_order.DrinkItemOrder));

                    _ingredientsViewers[0].SetDefault(_ingredientsConfig.GetSprite(_order.DrinkItemOrder));
                    _ingredientsViewers[0].gameObject.SetActive(true);
                    _ingredientsViewers[0].SetDefaultColor(Color.white);

                    /*_ingredients[0].sprite = _ingredientsConfig.GetSprite(_order.DrinkItemOrder);
                    _ingredients[0].color = Color.white;
                    _ingredients[0].gameObject.SetActive(true);#1#
                }
            }
            else
            {
                /*foreach (var ingredient in _ingredients)
                    ingredient.color = Color.white;#1#

                foreach (var ingredient in _ingredientsViewers)
                    ingredient.SetDefaultColor(Color.white);
            }


            /*foreach (var ingredient in _ingredients)
                ingredient.transform.localScale = Vector3.one;#1#


            foreach (var ingredient in _ingredientsViewers)
                ingredient.ResetDefaultScale();
        }

        Debug.Log("Стек меняется");
        if (_assemblyBurger.IngredientStack.Count == 0)
            return;

        if (_assemblyBurger.IngredientStack.Count > _recipes.ItemTypes.Count)
        {
            /*foreach (var ingredient in _ingredients)
                ingredient.color = Color.gray;#1#

            foreach (var ingredient in _ingredientsViewers)
                ingredient.SetDefaultColor(Color.gray);

            return;
        }

        var stackItems = _assemblyBurger.IngredientStack.Select(x => x.item.ItemType).ToList();
        stackItems.Reverse();

        /*for (int i = 0; i < _ingredients.Length; i++)
        {
            int recipeIndex = _recipes.ItemTypes.Count - 1 - i;

            if (i >= stackItems.Count)
            {
                Debug.Log($"{i}). Индекс выходит за пределы стека");
                continue;
            }

            if (stackItems[i] == _recipes.ItemTypes[recipeIndex])
            {
                Debug.Log("Правильно ");
                _ingredients[i].color = Color.gray;
                _elementSelector.UpdateSpacing(i);
                /*Debug.Log("след ингр " + i + (1));
                Debug.Log("куррент стэцк " + i);
                Debug.Log("стэцк " + stackItems.Count);#2#
                // _ingredients[i + 1].transform.localScale = new Vector3(1.3f, 1.3f);
            }
            else
            {
                foreach (var ingredient in _ingredients)
                    ingredient.color = Color.gray;
            }
        }#1#


        for (int i = 0; i < _ingredientsViewers.Length; i++)
        {
            int recipeIndex = _recipes.ItemTypes.Count - 1 - i;

            if (i >= stackItems.Count)
            {
                Debug.Log($"{i}). Индекс выходит за пределы стека");
                continue;
            }

            if (stackItems[i] == _recipes.ItemTypes[recipeIndex])
            {
                Debug.Log("Правильно ");
                /*_ingredients[i].color = Color.gray;
                _elementSelector.UpdateSpacing(i);#1#


                _ingredientsViewers[i].SetDefaultColor(Color.gray);
                _elementSelector.UpdateSpacing(i);


                Debug.Log("_ingredientsViewers[i+1] " + _ingredientsViewers[i + 1]);
                Debug.Log("_recipes.ItemTypes[recipeIndex   1] " + _recipes.ItemTypes[recipeIndex]);
                Debug.Log("СТЕУКШКА " +stackItems[i]+" , , ,  " +_recipes.ItemTypes[recipeIndex]);
                /*Debug.Log("recipeIndex-1] " + (recipeIndex - 1));
                Debug.Log("_recipes.ItemTypes[recipeInde-1] " +_recipes.ItemTypes[recipeIndex-1]);#1#

                if (stackItems[i] == _recipes.ItemTypes[recipeIndex])
                {
                    if ((recipeIndex - 1) >= 0)
                    {
                        _ingredientsViewers[i + 1]
                            .SetOutlineBackground(true,
                                _ingredientsConfig.GetOutlineSprite(_recipes.ItemTypes[recipeIndex - 1]));
                    }
                    else
                    {
                        Debug.Log("вышел за массив индексов");
                    }
                }


                /*if ((recipeIndex - 1) >= 0)
                {
                    _ingredientsViewers[i + 1]
                        .SetOutlineBackground(true,
                            _ingredientsConfig.GetOutlineSprite(_recipes.ItemTypes[recipeIndex - 1]));
                }
                else
                {
                    Debug.Log("вышел за массив индексов");
                }#1#


                /*_ingredientsViewers[i+1]
                    .SetOutlineBackground(true,
                        _ingredientsConfig.GetOutlineSprite(_recipes.ItemTypes[recipeIndex+1]));#1#
            }
            else
            {
                /*foreach (var ingredient in _ingredients)
                    ingredient.color = Color.gray;#1#


                foreach (var ingredient in _ingredientsViewers)
                    ingredient.SetDefaultColor(Color.gray);

                _ingredientsViewers[i].SetOutlineBackground(false, null);
            }
        }*/


    public void CheckIngredientsProgress()
    {
        if (_order != null)
        {
            if (_order.IsBurgerCompleted)
            {
                foreach (var ingredient in _ingredientsViewers)
                    ingredient.gameObject.SetActive(false);

                if (!_order.IsDrinkCompleted)
                {
                    Debug.Log("_order.DrinkItemOrder " + _order.DrinkItemOrder);
                    Debug.Log("Sprite  " + _ingredientsConfig.GetSprite(_order.DrinkItemOrder));

                    _ingredientsViewers[0].SetDefault(_ingredientsConfig.GetSprite(_order.DrinkItemOrder));
                    _ingredientsViewers[0].gameObject.SetActive(true);
                    _ingredientsViewers[0].SetDefaultColor(Color.white);
                }
            }
            else
            {
                foreach (var ingredient in _ingredientsViewers)
                    ingredient.SetDefaultColor(Color.white);
            }

            foreach (var ingredient in _ingredientsViewers)
                ingredient.ResetDefaultScale();
            
            
            foreach (var ingredient in _ingredientsViewers)
                ingredient.SetOutlineBackground(false, null);
        }

        Debug.Log("Стек меняется");
        if (_assemblyBurger.IngredientStack.Count == 0)
            return;

        if (_assemblyBurger.IngredientStack.Count > _recipes.ItemTypes.Count)
        {
            foreach (var ingredient in _ingredientsViewers)
                ingredient.SetDefaultColor(Color.gray);

            return;
        }

        var stackItems = _assemblyBurger.IngredientStack.Select(x => x.item.ItemType).ToList();
        stackItems.Reverse();

        int lastCorrectIndex = -1;

        for (int i = 0; i < _ingredientsViewers.Length; i++)
        {
            int recipeIndex = _recipes.ItemTypes.Count - 1 - i;

            if (i >= stackItems.Count)
            {
                Debug.Log($"{i}). Индекс выходит за пределы стека");
                continue;
            }

            if (stackItems[i] == _recipes.ItemTypes[recipeIndex])
            {
                Debug.Log("Правильно ");
                _ingredientsViewers[i].SetDefaultColor(Color.gray);
                _elementSelector.UpdateSpacing(i);
                lastCorrectIndex = i;
            }
            else
            {
                _ingredientsViewers[i].SetDefaultColor(Color.gray);
                _ingredientsViewers[i].SetOutlineBackground(false, null);
            }
        }

        // Включаем outline только для последнего правильного ингредиента
        if (lastCorrectIndex >= 0 && lastCorrectIndex + 1 < _ingredientsViewers.Length)
        {
            int nextIndex = lastCorrectIndex + 1;
            int nextRecipeIndex = _recipes.ItemTypes.Count - 1 - nextIndex;

            if (nextRecipeIndex >= 0)
            {
                _ingredientsViewers[nextIndex].SetOutlineBackground(true,
                    _ingredientsConfig.GetOutlineSprite(_recipes.ItemTypes[nextRecipeIndex]));
            }
            else
            {
                _ingredientsViewers[lastCorrectIndex].SetOutlineBackground(true,
                    _ingredientsConfig.GetOutlineSprite(_recipes.ItemTypes[lastCorrectIndex]));
            }
        }
    }
}