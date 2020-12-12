using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads the new scene city
/// </summary>
public class PlayButtonBehaviour : MonoBehaviour
{
   public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("City");
    }
}
