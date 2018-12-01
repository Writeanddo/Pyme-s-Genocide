using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionCounter : MonoBehaviour {
    public int numOfMinions;
    public int maxNum = 500;

    public bool canCreate() {
        return numOfMinions < maxNum;
    }
}
