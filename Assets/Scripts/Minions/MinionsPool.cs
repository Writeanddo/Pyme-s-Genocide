
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsPool : MonoBehaviour {
    [SerializeField] bool showDebugLogs = false;
    [SerializeField] int poolSize = 500;
    List<Minion> minions;

    Transform _transform;

    [SerializeField] List<Minion> minionPrefabs = new List<Minion>();


    class MinionTexture {
        public int num;
        public Texture tex;

        public MinionTexture(int num, Texture tex) {
            this.num = num;
            this.tex = tex;
        }
    }


    List<MinionTexture> minionTextures = new List<MinionTexture>();

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
                InstantiateCopy();
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
            if (minions.Count == 0 && !forceInstantiate)
            {
                if (showDebugLogs) Debug.LogWarning("La pool está vacía. No pueden instanciarse más minions");
            }
            else
            {
                if (minions.Count == 0)
                {
                    if (showDebugLogs) Debug.LogWarning("La pool está vacía. Se instanciará uno y se añadirá a la pool");
                    InstantiateCopy();
                }

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
            minion.explosive = false;
            minion.GetComponent<Rigidbody>().velocity = Vector3.zero;
            minion.transform.position = new Vector3(10000.0f, 10000.0f, 10000.0f);
            minion.transform.rotation = Quaternion.identity;
            minion.transform.SetParent(_transform, true);

            if (showDebugLogs) Debug.Log("Devolviendo minion. Tamaño del pool: " + minions.Count);

            minions.Add(minion);
        }
    }

    private void LoadMinions() {
        if(minionPrefabs.Count == 0) {
            minionPrefabs.Add(Resources.Load<Minion>("Bocado"));
            minionPrefabs.Add(Resources.Load<Minion>("CJ"));
            minionPrefabs.Add(Resources.Load<Minion>("Tago"));
        }
        if(minionTextures.Count == 0) {

            minionTextures.Add(new MinionTexture(0, Resources.Load("Textures/CJ/bocado_tex") as Texture));
            minionTextures.Add(new MinionTexture(0, Resources.Load("Textures/CJ/bocado_tex2") as Texture));
            minionTextures.Add(new MinionTexture(0, Resources.Load("Textures/CJ/bocado_tex3") as Texture));

            minionTextures.Add(new MinionTexture(1, Resources.Load("Textures/CJ/seli_tex") as Texture));
            minionTextures.Add(new MinionTexture(1, Resources.Load("Textures/CJ/seli_tex2") as Texture));
            minionTextures.Add(new MinionTexture(1, Resources.Load("Textures/CJ/seli_tex3") as Texture));

            minionTextures.Add(new MinionTexture(2, Resources.Load("Textures/Tago/tago_tex") as Texture));
            minionTextures.Add(new MinionTexture(2, Resources.Load("Textures/Tago/tago_tex2") as Texture));
            minionTextures.Add(new MinionTexture(2, Resources.Load("Textures/Tago/tago_tex3") as Texture));
        }
    }

    private List<MinionTexture> GetTexturesOf(int num) {
        List<MinionTexture> ret = new List<MinionTexture>();
        for(int i = 0; i < minionTextures.Count; i++) {
            if (minionTextures[i].num == num)
                ret.Add(minionTextures[i]);
        }
        return ret;
    }

    private void InstantiateCopy()
    {
        LoadMinions();

        int rand = Mathf.RoundToInt(Random.Range(0, 3));
        Minion minionPrefab = minionPrefabs[rand];

        Minion copy = Instantiate(minionPrefab, new Vector3(10000.0f, 10000.0f, 10000.0f), Quaternion.identity, _transform);
        copy.gameObject.name = "MINION " + minions.Count.ToString().PadLeft(4, '0');
        if(rand == 2) //Esto es porque solo va en Tago
        copy.GetComponentInChildren<SkinnedMeshRenderer>().materials[2].mainTexture = GetTexturesOf(rand)[Random.Range(0, 3)].tex;
        copy.gameObject.SetActive(false);

        minions.Add(copy);
    }
}
