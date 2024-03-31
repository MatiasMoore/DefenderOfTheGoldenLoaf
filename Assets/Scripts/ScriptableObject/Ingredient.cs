using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjs/Ingredient")]
public class Ingredient : ScriptableObject
{
    public enum IngredientType
    {
        gold, 
        flour,
        cabbage,
        meat,
        tomato,
        bread,
        breadSlice,
        loaf,
        hotDogBread,
        trash,
        hotDog,
        burger,
        sandwich,
        goldenLoaf
    }

    [SerializeField] 
    IngredientType _type;

    [SerializeField]
    private string _name;

    [SerializeField]
    private Sprite _icon;

    public IngredientType GetIngredientType() => _type;

    public Sprite GetIngredientIcon() => _icon;
}
