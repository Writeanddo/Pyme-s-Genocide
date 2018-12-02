using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour {

    private AudioSource m_audio;

    private void Awake() {
        m_audio = GetComponent<AudioSource>();
        if (!m_audio) {
            m_audio = this.gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(AudioClip clip) {
        AudioManager manager = AudioManager.GetInterfaceController();
        float volume = 1.0f;
        if (manager != null)
            volume = manager.m_soundVolume;
        m_audio.PlayOneShot(clip, volume);
    }

    public void PlaySoundLoop(AudioClip clip) {
        AudioManager manager = AudioManager.GetInterfaceController();
        float volume = 1.0f;
        if (manager != null)
            volume = manager.m_soundVolume;
        m_audio.clip = clip;
        m_audio.loop = true;
        m_audio.Play();
        m_audio.volume = volume;
    }

    public void StopSoundLoop() {
        m_audio.Stop();
    }
}
