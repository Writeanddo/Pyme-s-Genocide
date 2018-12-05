using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bocadoText;
    [SerializeField] TextMeshProUGUI cjText;
    [SerializeField] TextMeshProUGUI flufText;
    [SerializeField] TextMeshProUGUI tagoText;

    [SerializeField] Button btContinue;
    [SerializeField] Button btExit;

    public static int nBocados = 0;
    public static int nCJ = 0;
    public static int nFluf = 0;
    public static int nTago = 0;

    GameManager gm;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetSelectedGameObjectNextFrame(btContinue.gameObject));

        if (gm == null)
        {
            gm = FindObjectOfType<GameManager>();
        }

        btContinue.onClick.AddListener(ContinueAction);
        btExit.onClick.AddListener(ExitAction);
    }

    private void OnDisable()
    {
        btContinue.onClick.RemoveListener(ContinueAction);
        btExit.onClick.AddListener(ExitAction);
    }

    private void ContinueAction()
    {
        gm.UnPause();
    }

    private void ExitAction()
    {
        Application.Quit();
    }

    IEnumerator SetSelectedGameObjectNextFrame(GameObject button)
    {
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void minionCounter(string id)
    {
        switch (id)
        {
            case "Bocado": nBocados++;  bocadoText.text = "BOCADOS x" + nBocados;  break;
            case "CJ": nCJ++;  cjText.text = "CJ x" + nCJ; break;
            case "Tago": nTago++; tagoText.text = "TAGO x" + nTago; break;
            case "fluf": nFluf++; flufText.text = "FLUF x" + nFluf; break;
            default: break;
        }
    }
}
