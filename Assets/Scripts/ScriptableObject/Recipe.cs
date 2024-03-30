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

    public List<IngredientWithInstruction> GetRequiredIngredients()
    {
        return new List<IngredientWithInstruction>(_requiredInstructions);
    }

}
