using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{

    [SerializeField] Button btContinue;
    [SerializeField] Button btExit;

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
}
