using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip ambienMusic;

    [Header("SFx")]
    public AudioClip playerJump;
    public AudioClip[] playerRun;
    public AudioClip playerShoot;
    public AudioClip playerJetpack;
    public AudioClip absorberPush;
    public AudioClip absorberOut;
    public AudioClip absorberIn;
    public AudioClip absorberBark;
    public AudioClip poof;
    public AudioClip boing;
    public AudioClip death;
    public AudioClip[] thrownMinion;
    public AudioClip absorbed;
    public AudioClip proud;
    public AudioClip spaceshipCollected;
    public AudioClip[] squeaks;
    public AudioClip steps;
    public AudioClip[] brokenGlass;
    public AudioClip button;

    [Header("Scene")]
    public AudioSource musicAudioSource1;
    public AudioSource musicAudioSource2;
    bool isMute;
    bool m_audioSource1IsPlaying;
    float m_secondChange;

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        musicAudioSource1 = audioSources[0];
        musicAudioSource2 = audioSources[1];
        musicAudioSource1.Play();
        m_audioSource1IsPlaying = true;
    }

    void Update()
    {
        if (Camera.main != null)
            this.transform.position = Camera.main.transform.position;
    }

    public void PlayOneShot(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position, OptionsObject.Instance.sfxVolumeValue);
    }

    public void PlayOneShotDelayed(AudioClip clip, float delay)
    {
        StartCoroutine(Delay(clip, delay));
    }

    IEnumerator Delay(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayOneShot(clip, Camera.main.transform.position);
    }
}
