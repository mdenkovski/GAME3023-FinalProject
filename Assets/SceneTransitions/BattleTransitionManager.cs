using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class BattleTransitionManager : MonoBehaviour
{
    public UnityEvent onEnterEncounter;
    [SerializeField]
    float battleEntryDelay = 3.0f;
    public UnityEvent onExitEncounter;
    [SerializeField]
    float battleExitDelay = 3.0f;




    public void EnterEncounter()
    {
        StartCoroutine(DelayBattle());
    }

    IEnumerator DelayBattle()
    {
        onEnterEncounter.Invoke();
        yield return new WaitForSeconds(battleEntryDelay);
        transform.root.gameObject.SetActive(false);
        SceneManager.LoadScene("Play");
    }

    public void ExitEncounter()
    {
        onExitEncounter.Invoke();
        Invoke("DelayEnterScene", battleEntryDelay);
    }


    public void DelayEnterScene()
    {

        transform.root.gameObject.SetActive(true);
        SceneManager.LoadScene("City");
    }
}
