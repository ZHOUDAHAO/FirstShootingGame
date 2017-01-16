using UnityEngine;
using System.Collections;

public class CollideBolt : MonoBehaviour {
    public int isStrong;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary")
            return;
    }
}
