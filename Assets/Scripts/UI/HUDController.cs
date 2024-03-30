using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    private VisualElement _root;

    private Button _pauseButton;

    private VisualElement _recipesContainer;

    private void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        SetupElements();
        RegisterCallbacks();
    }

    private void OnDisable()
    {
        UnregisterCallbacks();
    }

    private void SetupElements()
    {
        _recipesContainer = _root.Q<VisualElement>("recipesContainer");
    }

    private void RegisterCallbacks()
    {
        // Register here callbacks
    }

    private void UnregisterCallbacks()
    {
        // Unregister here callbacks
    }
    
    // ------ Callback Handlers ------
    private void OnPauseButtonClicked()
    {
        // Some functionality
    }

    // ------ Add elements to HUD
    public void AddRecipeElement(Recipe recipe)
    { 
        // Создание объекта "recipe"
        VisualElement recipeElement = new VisualElement();
        recipeElement.name = "recipe";
        recipeElement.style.marginRight = 5;
        recipeElement.AddToClassList("order");
        _recipesContainer.Add(recipeElement);
        Debug.Log("ADDED");

        // Заполнение ингредиентов объекта "recipe"
        List<IngredientWithInstruction> requiredIngredients = recipe.GetRequiredIngredients();
        for (int i = 0; i < requiredIngredients.Count; i++)
        {
            VisualElement ingredient = new VisualElement();
            ingredient.name = "ingredient_" + i;
            ingredient.AddToClassList("ingredient");
            ingredient.style.left = i * 63 + 32;
            ingredient.style.top = 227;

            Background ingredientIcon = new Background();
            ingredientIcon.sprite = requiredIngredients[i].GetIngredient().GetIngredientIcon();
            ingredient.style.backgroundImage = ingredientIcon;
            
            recipeElement.Add(ingredient);
        }

        // Добавление картинки изготавливаемого блюда
        VisualElement foodImage = new VisualElement();
        foodImage.name = "foodImage";
        foodImage.AddToClassList("food");

        Background dishIcon = new Background();
        dishIcon.sprite = recipe.GetDishIcon();
        foodImage.style.backgroundImage = dishIcon;

        recipeElement.Add(foodImage);

        // Добавление Label
        Label timer = new Label();
        timer.name = "timer";
        timer.text = "15:01"; // ЗДЕСЬ ДОЛЖНО ЗАДАВАТЬСЯ ВРЕМЯ
        timer.AddToClassList("label");
        recipeElement.Add(timer);
    }

    private void RemoveRecipeElement()
    {

    }
}
