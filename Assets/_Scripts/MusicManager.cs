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

    //the max volume that we want the scene to start at
    [Range(0,1)]
    [SerializeField]
    float startingVolume;

    //store all the audio tracks available
    [SerializeField]
    AudioClip[] trackList;

    //how long the transitions take to occur
    [SerializeField]
    float transitionDuration = 3.0f;

    //coroutine monitors if we are already running the fading coroutine
    IEnumerator FadingInTrack = null;
    IEnumerator FadingOutAndInTrack = null;

    //enum to match the track list sounds
    public enum Track
    {
        Field,
        City
    }

    private void Start()
    {
        //subscribe to events
        var transitionManager = SpawnPoint.player.GetComponent<BattleTransitionManager>();
        transitionManager.onEnterEncounter.AddListener(FadeOutMusic);
        transitionManager.onExitEncounter.AddListener(FadeOutMusic);

        //fade in the starting music
        FadeInStartingMusic();
    }

    //switch to city music
    public void OnCityEnterHandler()
    {
        //PlayTrack(Track.City);
        FadeOutAndInTrack(Track.City, transitionDuration);
    }

    //switch to field music
    public void OnCityExitHandler()
    {
        FadeOutAndInTrack(Track.Field, transitionDuration);
    }

    /// <summary>
    /// play a specific clip based on its trackID
    /// </summary>
    /// <param name="trackID"></param>
    public void PlayTrack(MusicManager.Track trackID)
    {
        musicSource.clip = trackList[(int)trackID];
        musicSource.Play();
    }


    /// <summary>
    /// change to a new track and fade in to it
    /// </summary>
    /// <param name="trackID"></param>
    /// <param name="duration"></param>
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


    /// <summary>
    /// coroutine to fade in a track 
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
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


    /// <summary>
    /// fade out from current track and fade into the new track over a specified duration
    /// </summary>
    /// <param name="trackID"></param>
    /// <param name="duration"></param>
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


    /// <summary>
    /// coroutine that fades out the track and then switchs to the new track
    /// </summary>
    /// <param name="trackID"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
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
        //switch to new track and fade into it
        FadeInTrackOverSeconds(trackID, duration);
    }

    /// <summary>
    /// fade out the current music
    /// </summary>
    void FadeOutMusic()
    {
        StartCoroutine(FadeOutOverSecondsCoroutine(1.0f));
    }


    /// <summary>
    /// coroutine that handels fading out over x seconds
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
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

    /// <summary>
    /// fade in music at the beginning of a scene
    /// </summary>
    void FadeInStartingMusic()
    {
        StartCoroutine(FadeInStartOverSecondsCoroutine(2.0f));
    }


    /// <summary>
    /// fade in the music based on the specified starting volume
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
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

    /// <summary>
    /// unsubscibe from the events on destroy if valid
    /// </summary>
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
