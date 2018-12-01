using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private int ammo = 0;
    public Text text;

    public void DecreaseAmmo() {
        ammo--;
        SetText();
    }
    public void IncreaseAmmo() {
        ammo++;
        SetText();
    }
    private void SetText() { text.text = "Ammo: " + ammo; }

    public int GetAmmo() { return ammo; }
}
