using UnityEngine;
using UnityEngine.UIElements;

namespace DefenderOfTheGoldenLoaf.UI
{
    public class UIController : MonoBehaviour
    {
        private VisualElement _root;

        private Button _playButton;
        private Button _settingsButton;
        private Button _exitButton;

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
            _playButton = _root.Q<Button>("playButton");
            _settingsButton = _root.Q<Button>("settingsButton");
            _exitButton = _root.Q<Button>("exitButton");
        }

        private void RegisterCallbacks()
        {
            _playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
            _settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
            _exitButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
        }

        private void UnregisterCallbacks()
        {
            _playButton.UnregisterCallback<ClickEvent>(OnPlayButtonClicked);
            _settingsButton.UnregisterCallback<ClickEvent>(OnSettingsButtonClicked);
            _exitButton.UnregisterCallback<ClickEvent>(OnExitButtonClicked);
        }

        // ------ Callbacks Handlers ------
        private void OnPlayButtonClicked(ClickEvent evt)
        {
            Debug.Log("Play button clicked!");
        }

        private void OnSettingsButtonClicked(ClickEvent evt)
        {
            Debug.Log("Settings button clicked!");
        }

        private void OnExitButtonClicked(ClickEvent evt)
        {
            Debug.Log("Exit button clicked!");
        }
    }
}
