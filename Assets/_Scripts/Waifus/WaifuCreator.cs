using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// WaifuCreator is a Scriptable Object at holds the basic stats and framework for each Waifu
/// </summary>
[CreateAssetMenu(fileName = "Waifu", menuName = "ScriptableObjects/Waifu", order = 3)]
public class WaifuCreator : ScriptableObject
{

    public Sprite characterImage;

    [SerializeField]
    private string characterName;
    // how much health points they will have
    [SerializeField]
    private int healthMax;
    // defence how much damage they will block from each attack
    [SerializeField]
    private int defence;
    // attack is how much damage they will deal
    [SerializeField]
    private int attack;
    // love dictates how much they will recover with the rest action
    [SerializeField]
    private int love;
    // Aggressiveness make it more likely they are to attack even at lower health percentages. 
    // A value of 0 means neutral not more likely or less likely.
    // This value dictates how they think their health percentage is.
    // A 1 in this means they think of their health as double to what it is when determining a course of action.
    [SerializeField]
    private float aggressiveness;

    // Abilities the Waifu will have
    [SerializeField]
    private AbilityList myAbilities;

    public string CharacterName
    {
        get { return characterName; }
    }


    public int HealthMax
    {
        get { return healthMax; }
    }

    public int Defence
    {
        get { return defence; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int Love
    {
        get { return love; }
    }

    public float Aggressiveness
    {
        get { return aggressiveness; }
    }

    public AbilityList MyAbilties
    {
        get { return myAbilities; }
    }

}
