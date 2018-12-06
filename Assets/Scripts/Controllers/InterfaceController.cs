using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{

    GameManager gm;

    private int maxMinions;

    //[Header("Menu")]

    [Header("Ingame")]
    public Image imageCount;
    public TextMeshProUGUI text;

    [SerializeField] Image[] objectives;
    [SerializeField] Sprite[] lockedObjectivesSprites;
    [SerializeField] Sprite[] unlockedObjectivesSprites;

    [SerializeField] Image fadePanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject inGameOptionsPanel;

    [SerializeField] Slider sliderSfx;
    [SerializeField] Slider sliderMusic;
    [SerializeField] Slider sliderSensibility;

    private HashSet<int> unlockedObjectives = new HashSet<int>();

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        UpdateCounter();

        if (pausePanel != null)
        {
            sliderSfx.value = OptionsObject.Instance.sfxVolumeValue;
            sliderMusic.value = OptionsObject.Instance.musicVolumeValue;
            sliderSensibility.value = OptionsObject.Instance.sensibility;
        }
    }

    public void UpdateCounter()
    {
        float factor = gm.GetAmmo() / gm.MaxAmmo;
        imageCount.fillAmount = factor;
        text.text = ((int)(factor * 100)).ToString() + "%";

        Color c = Color.Lerp(
           new Color(85.0f, 192.0f, 157.0f, 255.0f) / 255.0f,
           //new Color(192.0f, 85.0f, 85.0f, 255.0f) / 255.0f,
           Color.red,
           1.0f - factor);

        imageCount.color = c;
        text.color = c;
    }

    public void UpdateSfxVolume()
    {
        OptionsObject.Instance.sfxVolumeValue = sliderSfx.value;
    }

    public void UpdateMusicVolume()
    {
        OptionsObject.Instance.musicVolumeValue = sliderMusic.value;
        gm.audioManager.musicAudioSource1.volume = sliderMusic.value;
        gm.audioManager.musicAudioSource2.volume = sliderMusic.value;
    }

    // TODO
    public void UpdateSensibilty()
    {
        OptionsObject.Instance.sensibility = sliderSensibility.value;
    }

    public void UpdateObjective(int objective)
    {
        Assert.IsTrue(objective >= 0 && objective < objectives.Length, "Out of bounds -> " + objective);

        objectives[objective].sprite = unlockedObjectivesSprites[objective];
        objectives[objective].color = new Color(1, 1, 1, 1);
        unlockedObjectives.Add(objective);

        if (unlockedObjectives.Count == objectives.Length)
        {
            gm.AllObjectivesCleared();
        }
    }

    public void FadeOutScreen(float time)
    {
        StartCoroutine(FadeOut(time));
    }

    public void FadeInScreen(float time)
    {
        StartCoroutine(FadeIn(time));
    }

    private IEnumerator FadeIn(float time)
    {
        Color faceColor = fadePanel.color;

        for (float t = 1f; t > 0; t -= Time.deltaTime / time)
        {
            faceColor.a = t;
            fadePanel.color = faceColor;
            yield return null;
        }

        faceColor.a = 0;
        fadePanel.color = faceColor;
    }

    private IEnumerator FadeOut(float time)
    {
        Color faceColor = fadePanel.color;

        for (float t = 0f; t < 1f; t += Time.deltaTime / time)
        {
            faceColor.a = t;
            fadePanel.color = faceColor;
            yield return null;
        }

        faceColor.a = 1;
        fadePanel.color = faceColor;
    }
}
