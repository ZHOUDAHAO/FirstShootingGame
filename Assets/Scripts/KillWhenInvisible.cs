using UnityEngine;
using System.Collections;

public class KillWhenInvisible : MonoBehaviour {
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
