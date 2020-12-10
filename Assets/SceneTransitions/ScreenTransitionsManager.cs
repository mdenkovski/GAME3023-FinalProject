using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransitionsManager : MonoBehaviour
{
    [SerializeField]
    Animator screenEffectsCanvasAnimator;


    private ScreenTransitionsManager() { }
    private static ScreenTransitionsManager instance;
    private ScreenTransitionsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = this;
            }
            return instance;
        }
    }


    void Start()
    {
        // subscribe to events
        var transitionManager = SpawnPoint.player.GetComponent<BattleTransitionManager>();
        transitionManager.onEnterEncounter.AddListener(onEnterEncounterHandler);
        transitionManager.onExitEncounter.AddListener(onExitEncounterHandler);

        SceneManager.sceneLoaded += OnEnterNewScene;


        //Ensure persistance and only one instance
        ScreenTransitionsManager[] sceneTransitionManager = FindObjectsOfType<ScreenTransitionsManager>();
        foreach (ScreenTransitionsManager mgr in sceneTransitionManager)
        {
            if (mgr != Instance)
            {
                Destroy(mgr.gameObject);
            }
        }



        DontDestroyOnLoad(transform.root);
    }

    public void onEnterEncounterHandler()
    {
        screenEffectsCanvasAnimator.Play("FadeOut");
    }

    void onExitEncounterHandler()
    {
        screenEffectsCanvasAnimator.Play("FadeOut");
    }

    void OnEnterNewScene(Scene newScene, LoadSceneMode newSceneMode)
    {
        screenEffectsCanvasAnimator.Play("FadeIn");
    }
}
