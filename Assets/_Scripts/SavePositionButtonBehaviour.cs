using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Save Position Button Behaviour Invokes the OnSave Event in the PlayerController to save the players information
/// </summary>
public class SavePositionButtonBehaviour : MonoBehaviour
{
    public static UnityEvent OnSave = new UnityEvent();

    public void OnSaveButtonPressed()
    {
        Debug.Log("Save Button pressed");
        SaveLocation();
    }

    void SaveLocation()
    {
        OnSave.Invoke();
    }
}
