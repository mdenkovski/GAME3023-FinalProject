using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class BattleButtonBehaviour : MonoBehaviour
{
    public void OnBattleButtonPressed()
    {
        Debug.Log("Battle Button Pressed");
        SceneManager.LoadScene("Play");
    }
}
