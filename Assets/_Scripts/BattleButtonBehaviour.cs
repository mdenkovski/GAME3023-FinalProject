using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class BattleButtonBehaviour : MonoBehaviour
{
    public void OnBattleButtonPressed()
    {
        SpawnPoint.player.GetComponent<BattleTransitionManager>().EnterEncounter();
    }
}
