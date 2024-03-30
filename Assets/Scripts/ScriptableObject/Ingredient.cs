using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjs/Ingredient")]
public class Ingredient : ScriptableObject
{
    enum IngredientType
    {
        gold, 
        flour,
        eggs
    }

    [SerializeField] 
    IngredientType _type;

    [SerializeField]
    private string _name;

    [SerializeField]
    private Sprite _icon;
}
