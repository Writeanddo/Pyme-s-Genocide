using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FirstButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().Select();
	}
}
