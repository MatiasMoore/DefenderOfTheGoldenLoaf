using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    private VisualElement _root;

    private Button _pauseButton;
    private Button _continueButton;
    private Button _exitButton;

    private ProgressBar _levelProgressBar;

    private VisualElement _recipesContainer;
    private VisualElement _pauseMenu;

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
        _pauseMenu = _root.Q<VisualElement>("pauseMenu");

        _pauseButton = _root.Q<Button>("pauseButton");
        _continueButton = _root.Q<Button>("continueButton");
        _exitButton = _root.Q<Button>("exitButton");

        _levelProgressBar = _root.Q<ProgressBar>("levelProgressBar");
    }

    private void RegisterCallbacks()
    {
        _pauseButton.RegisterCallback<ClickEvent>(OnPauseButtonClicked);
        _continueButton.RegisterCallback<ClickEvent>(OnContinueButtonClicked);
        _exitButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
    }

    private void UnregisterCallbacks()
    {
        _pauseButton.UnregisterCallback<ClickEvent>(OnPauseButtonClicked);
        _continueButton.UnregisterCallback<ClickEvent>(OnContinueButtonClicked);
        _exitButton.UnregisterCallback<ClickEvent>(OnExitButtonClicked);
    }
    
    // ------ Callback Handlers ------
    private void OnPauseButtonClicked(ClickEvent evt)
    {
        Time.timeScale = 0;
        _pauseMenu.style.display = DisplayStyle.Flex;

    }

    private void OnContinueButtonClicked(ClickEvent evt)
    {
        Time.timeScale = 1;
        _pauseMenu.style.display = DisplayStyle.None;
    }

    private void OnExitButtonClicked(ClickEvent evt)
    {
        SceneManager.LoadScene(2);
    }

    // ------ Adding/Removing elements ------
    public VisualElement AddRecipeElement(Recipe recipe)
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
        timer.text = "placeholder";
        timer.AddToClassList("label");
        recipeElement.Add(timer);

        return recipeElement;
    }

    public void RemoveRecipeElement(VisualElement recipe)
    {
        recipe.RemoveFromHierarchy();
    }

    public void ChangeLevelProgressBarValue(float value)
    {
        _levelProgressBar.value = value;
    }
}
