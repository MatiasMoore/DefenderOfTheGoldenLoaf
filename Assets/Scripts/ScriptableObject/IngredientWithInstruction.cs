using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjs/IngredientWithInstruction")]
public class IngredientWithInstruction : ScriptableObject
{
    enum IngredientInstruction
    {
        none, bake, cut
    }

    [SerializeField]
    private Ingredient _ingredient;

    [SerializeField] 
    private IngredientInstruction _instruction;

    [SerializeField]
    private string _instructionName;
}
