using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the Scriptable object hold a list of Waifus and their Details
/// </summary>
[CreateAssetMenu(fileName = "WaifuList", menuName = "ScriptableObjects/WaifuList", order = 4)]
public class WaifuMasterList : ScriptableObject
{
    public WaifuCreator[] waifuList;
   
}
