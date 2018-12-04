using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    [SerializeField] float fadeTime = 2f;
    public Image fadePanel;
    public GameObject m_panelAnyKey;
    public GameObject m_panelMenu;
    public Slider m_musicVolume;
    public Slider m_effectsVolume;
    public SplineController splineController;

    private bool m_musicSelected = false;
    private bool m_effectsSelected = false;
    private UISoundEffects m_sound;
    private bool waitAnyKey = true;

    float counter = 72f;

    GameManager gm;

    void Start () {
        gm = FindObjectOfType<GameManager>();

        StartCoroutine(UnFading());

        AudioManager audio = gm.audioManager;

        if (audio != null) {
            m_musicVolume.value = audio.m_musicVolume;
            m_effectsVolume.value = audio.m_soundVolume;
        }
        m_sound = GetComponent<UISoundEffects>();
        m_musicSelected = false;
        m_effectsSelected = false;
    }

    void Update () {
        counter += Time.deltaTime;
        if (counter > 75f) {
            counter = 0;
            splineController.FollowSpline();
        }

        float axisHorizontal = Input.GetAxis("Horizontal");
        if (m_musicSelected) {
            m_musicVolume.value += axisHorizontal * 0.1f;
        }
        if (m_effectsSelected) {
            m_effectsVolume.value += axisHorizontal * 0.1f;
        }

        if (waitAnyKey) {
            if (Input.anyKey) {
                waitAnyKey = false;
                m_panelAnyKey.SetActive(false);
                m_panelMenu.SetActive(true);
            }
        }

        if (CrossPlatformInputManager.GetButtonDown("Cancel")) {
           /* if (m_controllerPanel.activeSelf) {
                m_controllerPanel.SetActive(false);
                m_buttonsPanel.SetActive(true);
                m_eventSystem.SetSelectedGameObject(m_controllerButton);
            } else if (m_buttonsPanel.activeSelf) {
                m_optionsPanel.SetActive(false);
                m_previousPanel.SetActive(true);
                m_eventSystem.SetSelectedGameObject(m_previousButton);
            }*/
        }
    }

    public void Play() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ChangeMusicVolume() {
        AudioManager audio = gm.audioManager;
        if (audio != null) {
            audio.ChangeMusicVolume(m_musicVolume.value);
        }
    }

    public void ChangeEffectsVolume() {
        AudioManager audio = gm.audioManager;
        if (audio != null) {
            audio.ChangeEffectsVolume(m_effectsVolume.value);
        }
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
        m_panelAnyKey.gameObject.SetActive(true);
    }
    IEnumerator UnFading() {
        for (float t = fadeTime; t > 0.0f;) {
            t -= Time.deltaTime;
            fadePanel.color = new Color(0f, 0f, 0f, t / (fadeTime));
            yield return null;
        }
    }
}
