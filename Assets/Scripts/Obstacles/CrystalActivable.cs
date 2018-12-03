using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalActivable : MonoBehaviour {

    [SerializeField] bool activated;
    [SerializeField] Material[] materials;
    [SerializeField] Material lineCrystal;
    public IActivable[] activableObjects;


    private float cooldown = 0.5f;
    private bool switchMode = true;

    private Renderer render;
    
    void Start () {
        render = GetComponent<Renderer>();
        SetMaterials();
    }

    private void SetMaterials(bool change = false)
    {
        if (change) activated = !activated;
        var mats = render.materials;

        if (activated) mats[0] = materials[0];
        else mats[0] = materials[1];

        mats[1] = lineCrystal;
        render.materials = mats;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Minion")
        {
            if (switchMode)
            {
                switchMode = false;
                SetMaterials(true);
                foreach (IActivable activableObject in activableObjects)
                {
                    if (activableObject)
                    {
                        activableObject.Activate();
                    }
                }
                Invoke("SwitchModeCooldown", cooldown);
            }
            Destroy(collision.gameObject);
        }
    }

    private void SwitchModeCooldown()
    {
        switchMode = true;
    }

}
