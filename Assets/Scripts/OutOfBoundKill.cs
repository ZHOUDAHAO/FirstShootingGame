using UnityEngine;
using System.Collections;

public class OutOfBoundKill : MonoBehaviour {
    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
