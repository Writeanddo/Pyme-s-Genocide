using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{

    GameManager gm;

    private int maxMinions;

    //[Header("Menu")]

    [Header("Ingame")]
    public Text textCount;
    public Image imageCount;
    public Image imageVerticalCount;

    [SerializeField] Image[] objectives;
    [SerializeField] Sprite[] lockedObjectivesSprites;
    [SerializeField] Sprite[] unlockedObjectivesSprites;

    private HashSet<int> unlockedObjectives = new HashSet<int>();

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void Initialize(int max, int count = 0)
    {
        maxMinions = max;
        UpdateCounter(count);
    }

    public void UpdateCounter(float count)
    {
        textCount.text = (int)count + "%";
        imageVerticalCount.fillAmount = imageCount.fillAmount = count / maxMinions;
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
}
