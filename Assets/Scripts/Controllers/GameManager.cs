using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum gameState { menu, ingame, paused}
    private gameState currentGameState = gameState.ingame;

    public InterfaceController interfaceController;
    public AudioManager audioManager;


    [SerializeField] float maxAmmo = 10000.0f;
    public float MaxAmmo { get { return maxAmmo; } }

    [SerializeField] float ammo;

    private void Start()
    {
        // ammo = maxAmmo;
        interfaceController.Initialize((int)maxAmmo, (int)ammo);
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
