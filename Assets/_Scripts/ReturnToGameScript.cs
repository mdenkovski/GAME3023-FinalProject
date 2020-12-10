using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToGameScript : MonoBehaviour
{
    public void OnReturnButtonPressed()
    {
        SpawnPoint.player.GetComponent<BattleTransitionManager>().ExitEncounter();
    }
}
