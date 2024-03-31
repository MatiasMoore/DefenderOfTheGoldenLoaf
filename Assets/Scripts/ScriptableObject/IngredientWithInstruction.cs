using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Ingredient;

[CreateAssetMenu(menuName = "ScriptableObjs/IngredientWithInstruction")]
public class IngredientWithInstruction : ScriptableObject
{
    public enum IngredientInstruction
    {
        none, chopped, cooked
    }

    [SerializeField]
    private Ingredient _ingredient;

    [SerializeField] 
    private IngredientInstruction _instruction;

    [SerializeField]
    private string _instructionName;

    [SerializeField]
    private Sprite _icon;

    public Sprite GetIcon() => _icon;

    public Ingredient.IngredientType GetIngredientType() => _ingredient.GetIngredientType();

    public IngredientInstruction GetIngredientInstruction() => _instruction;

    public Ingredient GetIngredient() => _ingredient;

    public bool IsEquivalentTo(IngredientWithInstruction other)
    {
        return this.GetIngredientType() == other.GetIngredientType() 
            && this.GetIngredientInstruction() == other.GetIngredientInstruction();
    }
}
