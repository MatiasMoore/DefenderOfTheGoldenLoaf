using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [SerializeField] private List<Recipe> _recipes;
    private Dictionary<int, VisualElement[]> _pagesWithRecipes = new Dictionary<int, VisualElement[]>();
    private int _currentPageNumber;
    private int _pagesNumber;

    private VisualElement _root;

    private Button _pauseButton;
    private Button _recipeBookButton;
    private Button _pauseMenuContinueButton;
    private Button _explanationPanelInPauseMenuButton;
    private Button _exitPauseMenuButton;
    private Button _explanationPanelContinueButton;
    private Button _exitWinPanelButton;
    private Button _tryAgainButton;
    private Button _exitLosePanelButton;
    private Button _recipeBookCloseButton;
    private Button _recipeBookNextButton;
    private Button _recipeBookPrevButton;

    private VisualElement _recipesContainer;
    private VisualElement _pauseMenu;
    private VisualElement _explanationPanel;
    private VisualElement _winPanel;
    private VisualElement _losePanel;
    private VisualElement _recipeBook;

    private ProgressBar _levelProgressBar;

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
        _recipeBook = _root.Q<VisualElement>("recipeBook");

        _pauseButton = _root.Q<Button>("pauseButton");
        _recipeBookButton = _root.Q<Button>("recipeBookButton");
        _pauseMenuContinueButton = _pauseMenu.Q<Button>("continueButton");
        _explanationPanelInPauseMenuButton = _pauseMenu.Q<Button>("explanationButton");
        _exitPauseMenuButton = _pauseMenu.Q<Button>("exitButton");
        _explanationPanelContinueButton = _explanationPanel.Q<Button>("continueButton");
        _exitWinPanelButton = _winPanel.Q<Button>("exitButton");
        _tryAgainButton = _losePanel.Q<Button>("tryAgainButton");
        _exitLosePanelButton = _losePanel.Q<Button>("exitButton");
        _recipeBookCloseButton = _recipeBook.Q<Button>("closeButton");
        _recipeBookPrevButton = _recipeBook.Q<Button>("prevButton");
        _recipeBookNextButton = _recipeBook.Q<Button>("nextButton");

        _levelProgressBar = _root.Q<ProgressBar>("levelProgressBar");
    }

    private void RegisterCallbacks()
    {
        _pauseButton.RegisterCallback<ClickEvent>(OnPauseButtonClicked);
        _recipeBookButton.RegisterCallback<ClickEvent>(OnRecipeBookButtonClicked);
        _pauseMenuContinueButton.RegisterCallback<ClickEvent>(OnPauseMenuContinueButtonClicked);
        _explanationPanelContinueButton.RegisterCallback<ClickEvent>(OnExplanationPanelContinueButtonClicked);
        _explanationPanelInPauseMenuButton.RegisterCallback<ClickEvent>(OnExplanationPanelOpenButtonClicked);
        _exitPauseMenuButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
        _exitWinPanelButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
        _tryAgainButton.RegisterCallback<ClickEvent>(OnTryAgainButtonClicked);
        _exitLosePanelButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
        _recipeBookCloseButton.RegisterCallback<ClickEvent>(OnRecipeBookCloseButtonClicked);
        _recipeBookPrevButton.RegisterCallback<ClickEvent>(OnPrevPageButtonClicked);
        _recipeBookNextButton.RegisterCallback<ClickEvent>(OnNextPageButtonClicked);
    }

    private void UnregisterCallbacks()
    {
        _pauseButton.UnregisterCallback<ClickEvent>(OnPauseButtonClicked);
        _recipeBookButton.UnregisterCallback<ClickEvent>(OnRecipeBookButtonClicked);
        _pauseMenuContinueButton.UnregisterCallback<ClickEvent>(OnPauseMenuContinueButtonClicked);
        _explanationPanelContinueButton.UnregisterCallback<ClickEvent>(OnExplanationPanelContinueButtonClicked);
        _explanationPanelInPauseMenuButton.UnregisterCallback<ClickEvent>(OnExplanationPanelOpenButtonClicked);
        _exitPauseMenuButton.UnregisterCallback<ClickEvent>(OnExitButtonClicked);
        _exitWinPanelButton.UnregisterCallback<ClickEvent>(OnExitButtonClicked);
        _tryAgainButton.UnregisterCallback<ClickEvent>(OnTryAgainButtonClicked);
        _exitLosePanelButton.UnregisterCallback<ClickEvent>(OnExitButtonClicked);
        _recipeBookCloseButton.UnregisterCallback<ClickEvent>(OnRecipeBookCloseButtonClicked);
        _recipeBookPrevButton.UnregisterCallback<ClickEvent>(OnPrevPageButtonClicked);
        _recipeBookNextButton.UnregisterCallback<ClickEvent>(OnNextPageButtonClicked);
    }

    // ------ Callback Handlers ------
    private void OnPauseButtonClicked(ClickEvent evt)
    {
        Time.timeScale = 0;
        _pauseMenu.style.display = DisplayStyle.Flex;

    }

    private void OnRecipeBookButtonClicked(ClickEvent evt)
    {
        Time.timeScale = 0;
        FillRecipeBookWithRecipes();
        _recipeBook.style.display = DisplayStyle.Flex;
        _currentPageNumber = 0;
        _pagesNumber -= 1;
        VisualElement[] page = _pagesWithRecipes[_currentPageNumber];

        page[0].style.display = DisplayStyle.Flex;
        page[1].style.display = DisplayStyle.Flex;
    }

    private void OnRecipeBookCloseButtonClicked(ClickEvent evt)
    {
        Time.timeScale = 1;
        _recipeBook.style.display = DisplayStyle.None;
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
        SceneManager.LoadSceneAsync(0);
    }

    private void OnTryAgainButtonClicked(ClickEvent evt)
    {
        Time.timeScale = 0;
        SceneManager.LoadSceneAsync(1);
    }

    private void OnPrevPageButtonClicked(ClickEvent evt)
    {
        if (_currentPageNumber <= 0)
        {
            _currentPageNumber = 0;
        }

        if (_currentPageNumber > 0 )
        {
            _currentPageNumber -= 1;
        }
        else
        {
            _currentPageNumber = 0;
        }

        VisualElement[] page = _pagesWithRecipes[_currentPageNumber + 1];
        page[0].style.display = DisplayStyle.None;
        page[1].style.display = DisplayStyle.None;

        page = _pagesWithRecipes[_currentPageNumber];
        page[0].style.display = DisplayStyle.Flex;
        page[1].style.display = DisplayStyle.Flex;
    }

    private void OnNextPageButtonClicked(ClickEvent evt)
    {
        if (_currentPageNumber > _pagesNumber)
        {
            _currentPageNumber = _pagesNumber;
        }

        if (_currentPageNumber < _pagesNumber)
        {
            _currentPageNumber += 1;
        }
        else
        {
            _currentPageNumber = _pagesNumber;
        }

        VisualElement[] page = _pagesWithRecipes[_currentPageNumber - 1];
        page[0].style.display = DisplayStyle.None;
        page[1].style.display = DisplayStyle.None;

        page = _pagesWithRecipes[_currentPageNumber];
        page[0].style.display = DisplayStyle.Flex;
        page[1].style.display = DisplayStyle.Flex;
    }

    // ------ Elements functions ------
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
        timer.text = recipe.name;
        timer.style.fontSize = 21;
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

    private void FillRecipeBookWithRecipes()
    {
        _pagesNumber = (int)Math.Ceiling((double)_recipes.Count / 4);

        for(int i = 0; i < _pagesNumber; i++)
        {
            VisualElement leftPart = new VisualElement();
            VisualElement rightPart = new VisualElement();
            _recipeBook.Q<VisualElement>("recipeBookBackground").Add(leftPart);
            _recipeBook.Q<VisualElement>("recipeBookBackground").Add(rightPart);
            leftPart.AddToClassList("leftPart");
            rightPart.AddToClassList("rightPart");

            leftPart.style.top = 25;
            leftPart.style.left = 200;
            rightPart.style.top = 25;
            rightPart.style.left = 650;

            leftPart.style.display = DisplayStyle.None;
            rightPart.style.display = DisplayStyle.None;
            VisualElement[] page = { leftPart, rightPart };

            SetupPageWithRecipes(i, leftPart, rightPart);

            if(!_pagesWithRecipes.ContainsKey(i))
            {
                _pagesWithRecipes.Add(i, page);
            }
        }
    }

    private void SetupPageWithRecipes(int currentPageIndex, VisualElement leftPart, VisualElement rightPart)
    {
        if(currentPageIndex * 4 < _recipes.Count)
        {
            leftPart.Add(AddRecipeElement(_recipes[currentPageIndex * 4]));
        }

        if (currentPageIndex * 4 + 1 < _recipes.Count)
        {
            leftPart.Add(AddRecipeElement(_recipes[currentPageIndex * 4 + 1]));
        }

        if (currentPageIndex * 4 + 2 < _recipes.Count)
        {
            rightPart.Add(AddRecipeElement(_recipes[currentPageIndex * 4 + 2]));
        }

        if (currentPageIndex * 4 + 3 < _recipes.Count)
        {
            rightPart.Add(AddRecipeElement(_recipes[currentPageIndex * 4 + 3]));
        }
    }
}
