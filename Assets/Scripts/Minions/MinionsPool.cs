
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsPool : MonoBehaviour
{
    [SerializeField] bool showDebugLogs = false;
    [SerializeField] int poolSize = 1000;
    List<Minion> minions;

    Transform _transform;

    [SerializeField] Minion minionPrefab;

    private readonly object syncLock = new object();

    static MinionsPool instance;
    public static MinionsPool Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject pool = new GameObject("[MINIONS_POOL]");
                instance = pool.AddComponent<MinionsPool>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            minions = new List<Minion>(poolSize);
            _transform = transform;

            for (int i = 0; i < poolSize; i++)
            {
                if (minionPrefab == null)
                {
                    minionPrefab = Resources.Load<Minion>("Minion");
                }
                Minion copy = Instantiate(minionPrefab, new Vector3(10000.0f, 10000.0f, 10000.0f), Quaternion.identity, _transform);
                copy.gameObject.SetActive(false);
                minions.Add(copy);
            }

            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Minion Get(bool forceInstantiate = false)
    {
        lock (syncLock)
        {
            if (minions.Count == 0)
            {
                if (forceInstantiate)
                {
                    if (showDebugLogs) Debug.LogWarning("La pool está vacía. Se instanciará uno y se añadirá a la pool");
                }
                else
                {
                    if (showDebugLogs) Debug.LogWarning("La pool está vacía. No pueden instanciarse más minions");
                }
            }
            else
            {
                Minion minion = minions[0];
                minion.transform.parent = null;
                minion.gameObject.SetActive(true);
                minions.RemoveAt(0);

                if (showDebugLogs) Debug.Log("Solicitado minion. Tamaño del pool: " + minions.Count);

                return minion;
            }
            return null;
        }
    }

    public void Put(Minion minion)
    {
        lock (syncLock)
        {
            minion.gameObject.SetActive(false);
            minion.GetComponent<Rigidbody>().velocity = Vector3.zero;
            minion.transform.position = new Vector3(10000.0f, 10000.0f, 10000.0f);
            minion.transform.SetParent(_transform, true);

            if (showDebugLogs) Debug.Log("Devolviendo minion. Tamaño del pool: " + minions.Count);

            minions.Add(minion);
        }
    }
}
