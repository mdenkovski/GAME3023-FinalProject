using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// balle button used in debugging to transition from city to battle. button is not enabled in actual game
/// </summary>
public class BattleButtonBehaviour : MonoBehaviour
{
    public void OnBattleButtonPressed()
    {
        SpawnPoint.player.GetComponent<BattleTransitionManager>().EnterEncounter();
    }
}
