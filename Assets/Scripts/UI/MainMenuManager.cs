using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] float fadeTime = 2f;
    public Image fadePanel;
    public GameObject m_panelAnyKey;
    public GameObject m_panelMenu;

    public Slider m_musicVolume;
    public Slider m_effectsVolume;
    public Slider m_sensibility;

    public SplineController splineController;

    private bool waitAnyKey = true;

    float counter = 72f;

    GameManager gm;

    AudioSource[] bgMusic;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        bgMusic = FindObjectsOfType<AudioSource>();

        m_effectsVolume.value = OptionsObject.Instance.sfxVolumeValue;
        m_musicVolume.value = OptionsObject.Instance.musicVolumeValue;
        m_sensibility.value = OptionsObject.Instance.sensibility;

        StartCoroutine(UnFading());
    }

    public void SfxSliderChanged(float value)
    {
        OptionsObject.Instance.sfxVolumeValue = value;
    }

    public void MusicSliderChanged(float value)
    {
        OptionsObject.Instance.musicVolumeValue = value;
        for (int i = 0; i < bgMusic.Length; i++)
        {
            bgMusic[i].volume = value;
        }
    }

    public void SensibilitySliderChanged(float value)
    {
        OptionsObject.Instance.sensibility = value;
    }

    private void OnEnable()
    {
        m_effectsVolume.onValueChanged.AddListener(SfxSliderChanged);
        m_musicVolume.onValueChanged.AddListener(MusicSliderChanged);
        m_sensibility.onValueChanged.AddListener(SensibilitySliderChanged);
    }

    private void OnDisable()
    {
        m_effectsVolume.onValueChanged.RemoveListener(SfxSliderChanged);
        m_musicVolume.onValueChanged.RemoveListener(MusicSliderChanged);
        m_sensibility.onValueChanged.RemoveListener(SensibilitySliderChanged);
    }

    void Update()
    {

        counter += Time.deltaTime;
        if (counter > 75f)
        {
            counter = 0;
            splineController.FollowSpline();
        }

        if (waitAnyKey)
        {
            if (Input.anyKey)
            {
                waitAnyKey = false;
                m_panelAnyKey.SetActive(false);
                m_panelMenu.SetActive(true);
            }
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PreesQuitButton()
    {
        Application.Quit();
    }

    IEnumerator Fading()
    {
        for (float t = 0.0f; t < fadeTime;)
        {
            t += Time.deltaTime;
            fadePanel.color = new Color(0f, 0f, 0f, t / (fadeTime));
            yield return null;
        }
        m_panelAnyKey.gameObject.SetActive(true);
    }
    IEnumerator UnFading()
    {
        for (float t = fadeTime; t > 0.0f;)
        {
            t -= Time.deltaTime;
            fadePanel.color = new Color(0f, 0f, 0f, t / (fadeTime));
            yield return null;
        }
    }
}
