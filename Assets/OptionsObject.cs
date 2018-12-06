using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsObject : MonoBehaviour
{
    public float sfxVolumeValue = 1.0f;
    public float musicVolumeValue = 1.0f;
    public float sensibility = 4.0f;

    static OptionsObject instance;
    public static OptionsObject Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject optionsGo = new GameObject("[OPTIONS]");
                instance = optionsGo.AddComponent<OptionsObject>();
                instance.sfxVolumeValue = 1.0f;
                instance.musicVolumeValue = 1.0f;
                instance.sensibility = 0.5f;
            }
            return instance;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
