using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashedPotController : MonoBehaviour {     
    public Vector3[] childrenSpawnPos;
    public GameObject child1;
    public GameObject child2;

    void OnEnable ()
    {
        Invoke("Disable", 5f);
        child1.transform.localPosition = childrenSpawnPos[0];
        child2.transform.localPosition = childrenSpawnPos[1];
    }

    void Disable()
    {
        GameController.GC.numPots--;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
