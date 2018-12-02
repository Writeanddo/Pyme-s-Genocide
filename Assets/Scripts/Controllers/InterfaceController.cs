using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour {

    private static InterfaceController instance;
    public static InterfaceController GetInterfaceController() { return instance; }

    private int maxMinions;

    //[Header("Menu")]

    [Header("Ingame")]
    public Text textCount;
    public Image imageCount;
    public Image imageVerticalCount;


    void Awake () {
        instance = this;
	}


    public void Initialize(int max, int count = 0) {
        maxMinions = max;
        UpdateCounter(count);
    }

    public void UpdateCounter(float count)
    {
        textCount.text = (int)count + "%";
        imageVerticalCount.fillAmount = imageCount.fillAmount = count / maxMinions;
    }
}
