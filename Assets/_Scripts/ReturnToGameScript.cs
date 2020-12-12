using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToGameScript : MonoBehaviour
{
    //When button is pressed
    public void OnReturnButtonPressed()
    {
        //triger player exit encounter method to transition back to the city scene
        SpawnPoint.player.GetComponent<BattleTransitionManager>().ExitEncounter();
    }
}
