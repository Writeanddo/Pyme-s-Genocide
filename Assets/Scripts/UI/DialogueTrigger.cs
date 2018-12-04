using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;
    bool dialogueTriggered = false;
    GameObject player;
  //  GameObject dialogueCamera;
    public GameObject dialogueManager;

    void Start() {
        player = GameObject.Find("Player");
     //   dialogueCamera.SetActive(false);
        //dialogueManager = GameObject.Find("DialogueManager");
        dialogueManager.SetActive(false);
    }

    public void TriggerDialogue() {

        player.GetComponent<PlayerControllerRB>().inputEnabled = false;
        dialogueManager.SetActive(true);
       // dialogueCamera.SetActive(true);
        //Camera.main.gameObject.SetActive(false);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && !dialogueTriggered) {
            TriggerDialogue();
            dialogueTriggered = true;
        }
    }

}