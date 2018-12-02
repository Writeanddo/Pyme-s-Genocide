using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] float maxAmmo = 10000.0f;
    public float MaxAmmo { get { return maxAmmo; } }

    [SerializeField] float ammo;

    public Text text;

    private void Start()
    {
        // ammo = maxAmmo;
        SetText();
    }

    public void DecreaseAmmo(float ammount = 1.0f)
    {
        ammo = Mathf.Max(0, ammo - ammount);
        SetText();
    }

    public void IncreaseAmmo(int ammount = 1)
    {
        ammo = Mathf.Min(maxAmmo, ammo + 1);
        SetText();
    }

    private void SetText() { text.text = "Ammo: " + Mathf.CeilToInt(ammo); }

    public float GetAmmo() { return ammo; }
}
