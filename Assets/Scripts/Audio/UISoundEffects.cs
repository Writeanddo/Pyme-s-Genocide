using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundEffects : SoundEffect
{

    public static string m_resourcesPath = "Audio/UI/";
    public static string m_changeFile = "Change";

    private AudioClip m_change;

    void Start()
    {
        m_change = Resources.Load<AudioClip>(m_resourcesPath + m_changeFile);
    }

    /*
     * 
      public void PlayChange()
      {
          base.PlaySound(m_change);
      }
  */
}
