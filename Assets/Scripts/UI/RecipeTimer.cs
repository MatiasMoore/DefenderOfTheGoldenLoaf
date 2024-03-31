using UnityEngine;
using UnityEngine.UIElements;

public class RecipeTimer : MonoBehaviour
{
    private Timer _timer;
    private VisualElement _recipeElement;
    private Label _recipeTimerLabel;

    private void Update()
    {
        _timer.Tick();
        _recipeTimerLabel.text = _timer.FloatToString(_timer.GetTime);
    }

    public void SetupObject(VisualElement recipeElement, float time)
    {
        if (_recipeElement != null) { return; }
        _recipeElement = recipeElement;

        if (_timer != null) { return; }
        _timer = new Timer(time);

        _recipeTimerLabel = _recipeElement.Q<Label>("timer");
        _timer.StartTimer();
    }
}
