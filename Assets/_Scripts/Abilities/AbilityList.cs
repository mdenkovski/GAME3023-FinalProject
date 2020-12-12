using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AbilityList is used to hold Multiabilities that are related to each one another. ie. A move list or a Master AbilityList
/// </summary>
[CreateAssetMenu(fileName = "AbilityList", menuName = "ScriptableObjects/AbilityList", order = 2)]
public class AbilityList : ScriptableObject
{
    public Ability[] abilityList;

    public void AssignItemIDs()
    {
        for (int i = 0; i < abilityList.Length; i ++)
        {
            abilityList[i].AbilityId = i;
        }
    }
}
