using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoundsAudioTransition : MonoBehaviour
{
    //events that will be called
    public UnityEvent onEnterCity;
    public UnityEvent onExitCity;


    /// <summary>
    /// when the player enters the city bounds
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if a player enters the trigger
        if(collision.gameObject.tag == "Player")
        {
            onEnterCity.Invoke();
        }
    }


    /// <summary>
    /// when the player exits the city bounds
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        //check if a player exits the trigger
        if (collision.gameObject.tag == "Player")
        {
            onExitCity.Invoke();
        }
    }

}
