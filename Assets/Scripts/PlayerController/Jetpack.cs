
using System.Collections;
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [SerializeField] float m_strength;
    public float Strength { get { return m_strength; } }

    [SerializeField] int m_AmmoPerSecond;
    public int AmmoPerSecond { get { return m_AmmoPerSecond; } }

    [SerializeField] float m_PushForce;
    [SerializeField] float m_PushTorque;

    float minionsSpawned;
    int totalMinionsSpawned;

    Animator animatorL;
    Animator animatorR;

    [SerializeField] JetpackPipe jetpackL;
    [SerializeField] JetpackPipe jetpackR;

    GameManager gm;

    [SerializeField] AudioClip audioClipJetpackNoise;
    [SerializeField] AudioClip audioClipJetpackFailure;

    AudioSource audioSourceJetpackNoise;

    private float autoDeactivateWeaponTimer;

    public bool Ready { get; private set; }

    [SerializeField] float failureSoundDelay = 2.0f;
    bool failureSoundReady = true;

    PlayerControllerRB playerController;

    public bool BeingUsed { get; private set; }

    private void Start()
    {
        Ready = true;
        BeingUsed = false;
        gm = FindObjectOfType<GameManager>();
        animatorL = jetpackL.GetComponent<Animator>();
        animatorR = jetpackR.GetComponent<Animator>();
        audioSourceJetpackNoise = GetComponent<AudioSource>();
        playerController = FindObjectOfType<PlayerControllerRB>();
    }

    private void Update()
    {
        audioSourceJetpackNoise.volume = gm.audioManager.m_soundVolume;
    }

    public void SetReady()
    {
        Ready = true;
    }

    public void StartJetpack()
    {
        BeingUsed = true;
        animatorR.SetBool("active", true);
        animatorL.SetBool("active", true);

        if (!audioSourceJetpackNoise.isPlaying && gm.GetAmmo() > 0.0f)
        {
            if (audioSourceJetpackNoise.isPlaying)
            {
                audioSourceJetpackNoise.Stop();
            }
            audioSourceJetpackNoise.loop = true;
            audioSourceJetpackNoise.clip = audioClipJetpackNoise;
            audioSourceJetpackNoise.Play();
        }
    }

    public void CancelJetpack(bool outOfAmmo = false)
    {
        BeingUsed = false;
        animatorR.SetBool("active", false);
        animatorL.SetBool("active", false);
        AnimatorClipInfo[] clipInfo = animatorR.GetCurrentAnimatorClipInfo(0);
        if (clipInfo[0].clip.name == "LoopJetPack")
        {
            Ready = false;
        }

        if (audioSourceJetpackNoise.isPlaying && audioSourceJetpackNoise.clip == audioClipJetpackNoise)
        {
            audioSourceJetpackNoise.Stop();

            if (outOfAmmo)
            {
                PlayOutOfAmmoSfx();
            }
        }
    }

    public void PlayOutOfAmmoSfx()
    {
        if (!audioSourceJetpackNoise.isPlaying || audioSourceJetpackNoise.clip != audioClipJetpackFailure)
        {
            audioSourceJetpackNoise.loop = false;
            audioSourceJetpackNoise.clip = audioClipJetpackFailure;
            audioSourceJetpackNoise.Play();
        }
    }

    //true = L, false = R
    bool lastJetPack;

    public void SpawnMinions()
    {
        float t = Time.deltaTime;
        minionsSpawned += m_AmmoPerSecond * t;

        if (Mathf.Ceil(minionsSpawned) > totalMinionsSpawned)
        {
            int spawnAmmount = (int)Mathf.Ceil(minionsSpawned) - totalMinionsSpawned;

            for (int i = 0; i < spawnAmmount; i++)
            {
                Minion m = MinionsPool.Instance.Get(true);
                gm.AddMinionCounterToPauseMenu(m.minionID);
                m.explosive = true;
                m.DisableHarvestingForSeconds(1.0f);

                if (lastJetPack)
                {
                    m.transform.position = jetpackL.spawnPoint.position;
                }
                else
                {
                    m.transform.position = jetpackR.spawnPoint.position;
                }

                m.transform.localScale = 0.5f * Vector3.one;
                m.transform.rotation = Random.rotation;
                m.AddForce(m_PushForce * Vector3.down, ForceMode.Impulse);
                m.AddTorque(m_PushTorque * Random.insideUnitSphere, ForceMode.Impulse);

                lastJetPack = !lastJetPack;
            }
        }

        totalMinionsSpawned = (int)Mathf.Ceil(minionsSpawned);
    }
}
