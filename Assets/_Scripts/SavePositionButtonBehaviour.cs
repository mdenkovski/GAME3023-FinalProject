using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
