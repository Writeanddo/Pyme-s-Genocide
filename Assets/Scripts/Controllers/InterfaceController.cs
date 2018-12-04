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

    private HashSet<int> unlockedObjectives = new HashSet<int>();

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void UpdateCounter()
    {
        float factor = gm.GetAmmo() / gm.MaxAmmo;
        imageCount.fillAmount = factor;
        text.text = ((int)(factor * 100)).ToString();
    }

    public void UpdateObjective(int objective)
    {
        Assert.IsTrue(objective >= 0 && objective < objectives.Length, "Out of bounds -> " + objective);

        objectives[objective].sprite = unlockedObjectivesSprites[objective];
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
