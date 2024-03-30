using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ScriptableObjs/Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private GameObject _object;

    [SerializeField]
    private List<IngredientWithInstruction> _requiredInstructions = new();

    private bool _isFinished = false;

    private List<IngredientWithInstruction> _finishedInstructions = new();
    public static UnityAction<Recipe, IngredientWithInstruction> AddedInstructionToRecipe;
    public static UnityAction<Recipe> RecipeFinished;

    public bool AddFinishedIntruction(IngredientWithInstruction newInstruction)
    {
        if (!_requiredInstructions.Contains(newInstruction))  
            return false; 
        

        _finishedInstructions.Add(newInstruction);
        AddedInstructionToRecipe?.Invoke(this, newInstruction);
        _requiredInstructions.Remove(newInstruction);

        if (_requiredInstructions.Count == 0)
            FinishRecipe();

        return true;
    }

    public bool IsFinished() => _isFinished;

    private void FinishRecipe()
    {
        _isFinished = true;
        RecipeFinished?.Invoke(this);
    }

}
