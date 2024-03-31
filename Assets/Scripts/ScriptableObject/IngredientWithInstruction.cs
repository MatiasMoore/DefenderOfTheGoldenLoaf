using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjs/IngredientWithInstruction")]
public class IngredientWithInstruction : ScriptableObject
{
    public enum IngredientInstruction
    {
        none, bake, cut
    }

    [SerializeField]
    private Ingredient _ingredient;

    [SerializeField] 
    private IngredientInstruction _instruction;

    [SerializeField]
    private string _instructionName;

    public Ingredient.IngredientType GetIngredientType() => _ingredient.GetIngredientType();

    public IngredientInstruction GetIngredientInstruction() => _instruction;

    public Ingredient GetIngredient() => _ingredient;

    public bool IsEquivalentTo(IngredientWithInstruction other)
    {
        return this.GetIngredientType() == other.GetIngredientType() 
            && this.GetIngredientInstruction() == other.GetIngredientInstruction();
    }
}
