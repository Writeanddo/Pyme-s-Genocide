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

    // Use this for initialization
    void Start() {
        sentences = new Queue<string>();
    }

    private void Update() {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit")) {
            DisplayNextSentence();
        }
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
        GameObject.Find("Player").GetComponent<PlayerControllerRB>().inputEnabled = true;

        if (dialogSource.id == 99)
        {
            GameObject.Find("SpaceShip").GetComponent<SpaceShip>().Eject();
            //Application.Quit();
        }
    }

}
