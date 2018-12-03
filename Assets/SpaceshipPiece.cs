using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipPiece : MonoBehaviour
{
    [SerializeField] int pieceId;

    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gm.interfaceController.UpdateObjective(pieceId);
            gameObject.SetActive(false);

            gm.audioManager.PlayOneShot(gm.audioManager.spaceshipCollected, transform.position);
            gm.audioManager.PlayOneShotDelayed(gm.audioManager.proud, gm.audioManager.spaceshipCollected.length * 0.5f);
        }
    }
}
