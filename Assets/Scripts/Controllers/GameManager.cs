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

    PlayerControllerRB player;
    ThirdPersonCamera tpc;

    private void Start()
    {
        // ammo = maxAmmo;

        pauseCanvas = FindObjectOfType<PauseCanvas>();
        tpc = FindObjectOfType<ThirdPersonCamera>();

        if (pauseCanvas == null)
        {
            PauseCanvas pauseCanvasPrefab = Resources.Load<PauseCanvas>("pauseCanvas");
            pauseCanvas = Instantiate(pauseCanvasPrefab);
            pauseCanvas.gameObject.name = "[PauseCanvas]";
            pauseCanvas.gameObject.SetActive(false);
        }

        player = FindObjectOfType<PlayerControllerRB>();
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
        interfaceController.UpdateCounter();
    }

    public void IncreaseAmmo(int ammount = 1)
    {
        ammo = Mathf.Min(maxAmmo, ammo + 1);
        interfaceController.UpdateCounter();
    }

    public float GetAmmo() { return ammo; }

    public void AllObjectivesCleared()
    {
        Debug.Log("WASSUP");
    }

    public void Respawn()
    {
        StartCoroutine(RespawnAfterTime());
    }

    IEnumerator RespawnAfterTime()
    {
        player.inputEnabled = false;
        tpc.target = null;

        audioManager.PlayOneShot(audioManager.death, tpc.transform.position);


        yield return new WaitForSeconds(2.0f);

        interfaceController.FadeOutScreen(1.0f);
        yield return new WaitForSeconds(1.0f);

        ammo = 0.0f;
        interfaceController.UpdateCounter();
        player.transform.position = player.RespawnPosition;
        player.inputEnabled = true;
        tpc.target = player.transform;

        yield return new WaitForSeconds(1.0f);

        interfaceController.FadeInScreen(1.0f);
    }
}
