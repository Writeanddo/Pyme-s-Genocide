using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    [SerializeField] float fadeTime = 2f;
    public Image fadePanel;
    public Slider m_musicVolume;
    public Slider m_effectsVolume;
    private bool m_musicSelected = false;
    private bool m_effectsSelected = false;
    private UISoundEffects m_sound;

    void Start () {
        StartCoroutine(UnFading());

        AudioManager audio = AudioManager.GetInterfaceController();
        if (audio != null) {
            m_musicVolume.value = audio.m_musicVolume;
            m_effectsVolume.value = audio.m_soundVolume;
        }
        m_sound = GetComponent<UISoundEffects>();
        m_musicSelected = false;
        m_effectsSelected = false;
    }
	
	void Update () {
		
	}

    public void PreesQuitButton() {
        Application.Quit();
    }

    IEnumerator Fading() {
        for (float t = 0.0f; t < fadeTime;) {
            t += Time.deltaTime;
            fadePanel.color = new Color(0f, 0f, 0f, t / (fadeTime));
            yield return null;
        }
    }
    IEnumerator UnFading() {
        for (float t = fadeTime; t > 0.0f;) {
            t -= Time.deltaTime;
            fadePanel.color = new Color(0f, 0f, 0f, t / (fadeTime));
            yield return null;
        }
    }
}
