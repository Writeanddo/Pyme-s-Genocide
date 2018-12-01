using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsPool : MonoBehaviour
{
    [SerializeField] int poolSize = 1500;
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

    public Minion Get()
    {
        lock (syncLock)
        {
            if (minions.Count > 0)
            {
                Minion minion = minions[0];
                minion.gameObject.SetActive(true);
                minions.RemoveAt(0);

                Debug.Log("Solicitado minion. Tamaño del pool: " + minions.Count);
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
            minion.transform.position = new Vector3(10000.0f, 10000.0f, 10000.0f);
            minion.transform.SetParent(_transform, true);

            Debug.Log("Devolviendo minion. Tamaño del pool: " + minions.Count);

            minions.Add(minion);
        }
    }
}
