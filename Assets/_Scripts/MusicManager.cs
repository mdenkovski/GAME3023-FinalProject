using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// the current music files for field and city are from YouFulca 
/// </summary>

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioSource musicSource;

    [Range(0,1)]
    [SerializeField]
    float startingVolume;

    [SerializeField]
    AudioClip[] trackList;

    [SerializeField]
    float transitionDuration = 3.0f;

    IEnumerator FadingInTrack = null;
    IEnumerator FadingOutAndInTrack = null;

    public enum Track
    {
        Field,
        City
    }

    private void Start()
    {
        var transitionManager = SpawnPoint.player.GetComponent<BattleTransitionManager>();
        transitionManager.onEnterEncounter.AddListener(FadeOutMusic);
        transitionManager.onExitEncounter.AddListener(FadeOutMusic);

        FadeInStartingMusic();
    }
    public void OnCityEnterHandler()
    {
        //PlayTrack(Track.City);
        FadeOutAndInTrack(Track.City, transitionDuration);
    }

    public void OnCityExitHandler()
    {
        FadeOutAndInTrack(Track.Field, transitionDuration);
    }

    public void PlayTrack(MusicManager.Track trackID)
    {
        musicSource.clip = trackList[(int)trackID];
        musicSource.Play();
    }

    public void FadeInTrackOverSeconds(MusicManager.Track trackID, float duration)
    {

        //if coroutine is running
        if (FadingInTrack != null)
        {
            StopCoroutine(FadingInTrack);
        }
        musicSource.clip = trackList[(int)trackID];
        musicSource.Play();
        FadingInTrack = FadeInOverSecondsCoroutine(duration);
        StartCoroutine(FadingInTrack);

    }

    IEnumerator FadeInOverSecondsCoroutine(float duration)
    {
        musicSource.volume = 0.0f;

        float timer = 0.0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / duration;

            musicSource.volume = Mathf.SmoothStep(0, 1, normalizedTime); ;
            //fade volume
            yield return new WaitForEndOfFrame();

        }
    }

    public void FadeOutAndInTrack(MusicManager.Track trackID, float duration)
    {
        //if coroutine is running
        if (FadingOutAndInTrack != null)
        {
            StopCoroutine(FadingOutAndInTrack);
        }
        FadingOutAndInTrack = FadeOutAndInOverSecondsCoroutine(trackID, duration);
        StartCoroutine(FadingOutAndInTrack);
    }

    IEnumerator FadeOutAndInOverSecondsCoroutine(MusicManager.Track trackID, float duration)
    {
        var initialVolume = musicSource.volume;
        float timer = 0.0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / duration;

            musicSource.volume = Mathf.SmoothStep(initialVolume, 0, normalizedTime); ;
            //fade volume
            yield return new WaitForEndOfFrame();

        }

        FadeInTrackOverSeconds(trackID, duration);
    }

    void FadeOutMusic()
    {
        StartCoroutine(FadeOutOverSecondsCoroutine(1.0f));
    }

    IEnumerator FadeOutOverSecondsCoroutine(float duration)
    {
        var initialVolume = musicSource.volume;

        float timer = 0.0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / duration;

            musicSource.volume = Mathf.SmoothStep(initialVolume, 0, normalizedTime); ;
            //fade volume
            yield return new WaitForEndOfFrame();

        }

    }

    void FadeInStartingMusic()
    {
        StartCoroutine(FadeInStartOverSecondsCoroutine(2.0f));
    }

    IEnumerator FadeInStartOverSecondsCoroutine(float duration)
    {
        musicSource.volume = 0.0f;

        float timer = 0.0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / duration;

            musicSource.volume = Mathf.SmoothStep(0, startingVolume, normalizedTime); ;
            //fade volume
            yield return new WaitForEndOfFrame();

        }

    }


    private void OnDestroy()
    {
        if(SpawnPoint.player != null)
        {
            var transitionManager = SpawnPoint.player.GetComponent<BattleTransitionManager>();
            if (transitionManager)
            {
                transitionManager.onEnterEncounter.RemoveListener(FadeOutMusic);
                transitionManager.onExitEncounter.RemoveListener(FadeOutMusic);

            }
        }
       
        
    }
}
