using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public bool gamePaused { get; private set; }
    private bool canPause = true;
    PauseCanvas pauseCanvas;

    PlayerControllerRB player;
    ThirdPersonCamera tpc;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            interfaceController.gameObject.SetActive(false);
        }
        else
        {
            interfaceController.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        // ammo = maxAmmo;
        interfaceController.UpdateCounter();

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
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            AllObjectivesCleared();
        }
#endif

        if (SceneManager.GetActiveScene().name == "Main")
        {
            return;
        }

        if (Input.GetButtonDown("Exit") && canPause)
        {
            gamePaused = !gamePaused;
            if (gamePaused)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
        }
    }

    public void SwapPauseState()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            return;
        }

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

        if (SceneManager.GetActiveScene().name == "Main")
        {
            return;
        }

        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0.0f;
        pauseCanvas.gameObject.SetActive(true);
        gamePaused = true;
    }

    public void UnPause()
    {

        if (SceneManager.GetActiveScene().name == "Main")
        {
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;

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

    public GameObject dialogFinal;

    public void AllObjectivesCleared()
    {
        dialogFinal.SetActive(true);
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
        tpc.Restart();

        player.inputEnabled = true;
        tpc.target = player.transform;

        yield return new WaitForSeconds(1.0f);

        interfaceController.FadeInScreen(1.0f);
    }

    public void AddMinionCounterToPauseMenu(string id) { pauseCanvas.minionCounter(id); }
}
