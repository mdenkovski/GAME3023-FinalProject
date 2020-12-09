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

    [SerializeField]
    AudioClip[] trackList;

    [SerializeField]
    float transitionDuration = 3.0f;

    IEnumerator FadingTrack = null;

    public enum Track
    {
        Field,
        City
    }


    public void OnCityEnterHandler()
    {
        //PlayTrack(Track.City);
        FadeInTrackOverSeconds(Track.City, transitionDuration);
    }

    public void OnCityExitHandler()
    {
        FadeInTrackOverSeconds(Track.Field, transitionDuration);
    }

    public void PlayTrack(MusicManager.Track trackID)
    {
        musicSource.clip = trackList[(int)trackID];
        musicSource.Play();
    }

    public void FadeInTrackOverSeconds(MusicManager.Track trackID, float duration)
    {

        //if coroutine is running
        if (FadingTrack != null)
        {
            StopCoroutine(FadingTrack);
        }
        musicSource.clip = trackList[(int)trackID];
        musicSource.Play();
        FadingTrack = FadeInOverSecondsCoroutine(duration);
        StartCoroutine(FadingTrack);

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


}
