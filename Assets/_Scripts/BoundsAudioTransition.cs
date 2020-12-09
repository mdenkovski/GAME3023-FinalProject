using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoundsAudioTransition : MonoBehaviour
{

    public UnityEvent onEnterCity;
    public UnityEvent onExitCity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if a player enters the trigger
        if(collision.gameObject.tag == "Player")
        {
            onEnterCity.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //check if a player exits the trigger
        if (collision.gameObject.tag == "Player")
        {
            onExitCity.Invoke();
        }
    }

}
