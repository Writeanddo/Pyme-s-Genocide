using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager instance;
    public static AudioManager GetInterfaceController() { return instance; }

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

    [Header("Scene")]
    public float m_soundVolume = 1.0f;
    public float m_musicVolume = 1.0f;
    AudioSource musicAudioSource1;
    AudioSource musicAudioSource2;
    bool isMute;
    bool m_audioSource1IsPlaying;
    bool m_changeAudioSource = false;
    float m_secondChange;

    void Awake()
    {
        instance = this;
        return;

        DontDestroyOnLoad(this.gameObject);
        AudioSource[] audioSources = GetComponents<AudioSource>();
        musicAudioSource1 = audioSources[0];
        musicAudioSource2 = audioSources[1];
        musicAudioSource1.volume = m_musicVolume;
        musicAudioSource2.volume = m_musicVolume;
        musicAudioSource1.Play();
        m_audioSource1IsPlaying = true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Camera.main != null)
            this.transform.position = Camera.main.transform.position;
        if (m_changeAudioSource) {
            AudioSource audioSource1 = m_audioSource1IsPlaying ? musicAudioSource1 : musicAudioSource2;
            AudioSource audioSource2 = m_audioSource1IsPlaying ? musicAudioSource2 : musicAudioSource1;
            audioSource1.volume += Time.deltaTime / m_secondChange * m_musicVolume;
            audioSource2.volume -= Time.deltaTime / m_secondChange * m_musicVolume;
            if (audioSource2.volume <= 0 || audioSource1.volume >= m_musicVolume) {
                audioSource2.Stop();
                audioSource2.volume = 0;
                audioSource1.volume = m_musicVolume;
                m_changeAudioSource = false;
            }

        }
    }

    public void PlayMusic(AudioClip music, float timeChanged) {
        AudioSource audioSource = m_audioSource1IsPlaying ? musicAudioSource2 : musicAudioSource1;
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.volume = 0;
        audioSource.Play();
        m_secondChange = timeChanged;
        m_audioSource1IsPlaying = !m_audioSource1IsPlaying;
        m_changeAudioSource = true;
    }


    public void PlayMusicWithoutLoop(AudioClip music, float timeChanged) {
        AudioSource audioSource = m_audioSource1IsPlaying ? musicAudioSource2 : musicAudioSource1;
        audioSource.clip = music;
        audioSource.loop = false;
        audioSource.volume = 0;
        audioSource.Play();
        m_secondChange = timeChanged;
        m_audioSource1IsPlaying = !m_audioSource1IsPlaying;
        m_changeAudioSource = true;
    }

    public void ChangeMusicVolume(float volume) {
        m_musicVolume = volume;
        AudioSource audioSource = m_audioSource1IsPlaying ? musicAudioSource1 : musicAudioSource2;
        audioSource.volume = m_musicVolume;
    }

    public void ChangeEffectsVolume(float volume) {
        m_soundVolume = volume;
    }

    public void PlayOneShot(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position, m_soundVolume);
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
