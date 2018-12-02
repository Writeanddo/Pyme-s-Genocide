using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum gameState { menu, ingame, paused }
    private gameState currentGameState = gameState.ingame;

    public InterfaceController interfaceController;
    public AudioManager audioManager;


    [SerializeField] float maxAmmo = 10000.0f;
    public float MaxAmmo { get { return maxAmmo; } }

    [SerializeField] float ammo;

    bool gamePaused;

    PauseCanvas pauseCanvas;

    private void Start()
    {
        // ammo = maxAmmo;
        interfaceController.Initialize((int)maxAmmo, (int)ammo);

        pauseCanvas = FindObjectOfType<PauseCanvas>();

        if (pauseCanvas == null)
        {
            PauseCanvas pauseCanvasPrefab = Resources.Load<PauseCanvas>("pauseCanvas");
            pauseCanvas = Instantiate(pauseCanvasPrefab);
            pauseCanvas.gameObject.name = "[PauseCanvas]";
            pauseCanvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Exit"))
        {
            gamePaused = !gamePaused;
            if (gamePaused)
            {
                Time.timeScale = 0.0f;
                pauseCanvas.gameObject.SetActive(true);
            }
            else
            {
                Time.timeScale = 1.0f;
                pauseCanvas.gameObject.SetActive(false);
            }
        }
    }

    public void SwapPauseState()
    {
        if (gamePaused)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        pauseCanvas.gameObject.SetActive(true);
        gamePaused = true;
    }

    public void UnPause()
    {
        Time.timeScale = 1.0f;
        pauseCanvas.gameObject.SetActive(false);
        gamePaused = false;
    }

    public void DecreaseAmmo(float ammount = 1.0f)
    {
        ammo = Mathf.Max(0, ammo - ammount);
        interfaceController.UpdateCounter(ammo);
    }

    public void IncreaseAmmo(int ammount = 1)
    {
        ammo = Mathf.Min(maxAmmo, ammo + 1);
        interfaceController.UpdateCounter(ammo);
    }

    public float GetAmmo() { return ammo; }
}
