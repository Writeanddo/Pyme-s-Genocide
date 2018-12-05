using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
    static ParticleSystemManager instance;
    public static ParticleSystemManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject m = new GameObject("[PARTICLE_SYSTEM_MANAGER]");
                instance = m.AddComponent<ParticleSystemManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play(ParticleSystem ps, Transform parent)
    {
        if (ReferenceEquals(ps, null) || ps == null || ps.transform == null || ps.transform.parent == null)
        {
            Debug.Log("A");
        }

        ps.transform.parent = transform;
        ps.Play();

        StartCoroutine(Retach(ps, parent));
    }

    IEnumerator Retach(ParticleSystem ps, Transform parent)
    {
        while (ps.isPlaying)
        {
            yield return null;
        }

        if (parent == null || ReferenceEquals(parent, null))
        {
            Destroy(ps.gameObject);
        }
        else
        {
            ps.transform.parent = parent;
        }
    }
}
