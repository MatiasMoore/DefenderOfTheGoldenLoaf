using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    private Recipe firstRecipe;

    [SerializeField]
    private Recipe secondRecipe;

    [SerializeField]
    private HUDController controller;

    private void Start()
    {
        controller.AddRecipeElement(firstRecipe);
        controller.AddRecipeElement(secondRecipe);
    }
}
