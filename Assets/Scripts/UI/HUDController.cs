using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    private VisualElement _root;

    private Button _pauseButton;
    private Button _pauseMenuContinueButton;
    private Button _explanationPanelInPauseMenuButton;
    private Button _exitPauseMenuButton;
    private Button _explanationPanelContinueButton;
    private Button _exitWinPanelButton;
    private Button _tryAgainButton;
    private Button _exitLosePanelButton;

    private ProgressBar _levelProgressBar;

    private VisualElement _recipesContainer;
    private VisualElement _pauseMenu;
    private VisualElement _explanationPanel;
    private VisualElement _winPanel;
    private VisualElement _losePanel;

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
        _explanationPanel = _root.Q<VisualElement>("explanationPanel");
        _winPanel = _root.Q<VisualElement>("winPanel");
        _losePanel = _root.Q<VisualElement>("losePanel");

        _pauseButton = _root.Q<Button>("pauseButton");
        _pauseMenuContinueButton = _pauseMenu.Q<Button>("continueButton");
        _explanationPanelInPauseMenuButton = _pauseMenu.Q<Button>("explanationButton");
        _exitPauseMenuButton = _pauseMenu.Q<Button>("exitButton");
        _explanationPanelContinueButton = _explanationPanel.Q<Button>("continueButton");
        _exitWinPanelButton = _winPanel.Q<Button>("exitButton");
        _tryAgainButton = _losePanel.Q<Button>("tryAgainButton");
        _exitLosePanelButton = _losePanel.Q<Button>("exitButton");

        _levelProgressBar = _root.Q<ProgressBar>("levelProgressBar");
    }

    private void RegisterCallbacks()
    {
        _pauseButton.RegisterCallback<ClickEvent>(OnPauseButtonClicked);
        _pauseMenuContinueButton.RegisterCallback<ClickEvent>(OnPauseMenuContinueButtonClicked);
        _explanationPanelContinueButton.RegisterCallback<ClickEvent>(OnExplanationPanelContinueButtonClicked);
        _explanationPanelInPauseMenuButton.RegisterCallback<ClickEvent>(OnExplanationPanelOpenButtonClicked);
        _exitPauseMenuButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
        _exitWinPanelButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
        _tryAgainButton.RegisterCallback<ClickEvent>(OnTryAgainButtonClicked);
        _exitLosePanelButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
    }

    private void UnregisterCallbacks()
    {
        _pauseButton.UnregisterCallback<ClickEvent>(OnPauseButtonClicked);
        _pauseMenuContinueButton.UnregisterCallback<ClickEvent>(OnPauseMenuContinueButtonClicked);
        _explanationPanelContinueButton.UnregisterCallback<ClickEvent>(OnExplanationPanelContinueButtonClicked);
        _explanationPanelInPauseMenuButton.UnregisterCallback<ClickEvent>(OnExplanationPanelOpenButtonClicked);
        _exitPauseMenuButton.UnregisterCallback<ClickEvent>(OnExitButtonClicked);
        _exitWinPanelButton.UnregisterCallback<ClickEvent>(OnExitButtonClicked);
        _tryAgainButton.UnregisterCallback<ClickEvent>(OnTryAgainButtonClicked);
        _exitLosePanelButton.UnregisterCallback<ClickEvent>(OnExitButtonClicked);
    }

    // ------ Callback Handlers ------
    private void OnPauseButtonClicked(ClickEvent evt)
    {
        Time.timeScale = 0;
        _pauseMenu.style.display = DisplayStyle.Flex;

    }

    private void OnPauseMenuContinueButtonClicked(ClickEvent evt)
    {
        Time.timeScale = 1;
        _pauseMenu.style.display = DisplayStyle.None;
    }

    private void OnExplanationPanelContinueButtonClicked(ClickEvent evt)
    {
        if (_pauseMenu.style.display != DisplayStyle.Flex)
        {
            Time.timeScale = 1;
        }
        _explanationPanel.style.display = DisplayStyle.None;
    }

    private void OnExplanationPanelOpenButtonClicked(ClickEvent evt)
    {
        _explanationPanel.style.display = DisplayStyle.Flex;
    }

    private void OnExitButtonClicked(ClickEvent evt)
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(2);
    }

    private void OnTryAgainButtonClicked(ClickEvent evt)
    {
        Time.timeScale = 0;
        SceneManager.LoadSceneAsync(1);
    }

    // ------ Adding/Removing elements ------
    public VisualElement AddRecipeElement(Recipe recipe)
    {
        // Создание объекта "recipe"
        VisualElement recipeElement = new VisualElement();
        recipeElement.name = "recipe";
        recipeElement.style.marginBottom = 30;
        recipeElement.AddToClassList("order");
        _recipesContainer.Add(recipeElement);

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
            ingredientIcon.sprite = requiredIngredients[i].GetIcon();
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

    public void SetWinPanelActivity(bool isActive)
    {
        Time.timeScale = 0;

        if(isActive)
        {
            _winPanel.style.display = DisplayStyle.Flex;
            return;
        }
        
        _winPanel.style.display = DisplayStyle.None;
    }

    public void SetLosePanelActivity(bool isActive)
    {
        Time.timeScale = 0;

        if (isActive)
        {
            _losePanel.style.display = DisplayStyle.Flex;
            return;
        }

        _losePanel.style.display = DisplayStyle.None;
    }
}
