using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour {

    private static InterfaceController instance;
    public static InterfaceController GetInterfaceController() { return instance; }

    public Text textCount;
    public Image imageCount;

	
	void Awake () {
        instance = this;
	}

    public void UpdateCounter(float count)
    {
        textCount.text = (int)count + "%";
        imageCount.fillAmount = count/100f;
    }
}
