using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;
    public GameObject dialogue;

    private Queue<string> sentences;

    public Dialogue dialogSource;
    GameManager gm;

    AudioSource audioSource;

    // Use this for initialization
    void Start() {
        sentences = new Queue<string>();
        gm = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update() {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit")) {
            DisplayNextSentence();
        }

        audioSource.volume = OptionsObject.Instance.sfxVolumeValue;
    }

    public void StartDialogue(Dialogue dialogue) {

        dialogSource = dialogue;

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.clip = gm.audioManager.IA[Random.Range(0, gm.audioManager.IA.Length)];
        audioSource.Play();

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue() {
        dialogue.SetActive(false);
        audioSource.Stop();

        GameObject.Find("Player").GetComponent<PlayerControllerRB>().inputEnabled = true;

        if (dialogSource.id == 99)
        {
            GameObject.Find("SpaceShip").GetComponent<SpaceShip>().Eject();
            //Application.Quit();
        }
    }

}
