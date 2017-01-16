using UnityEngine;
using System.Collections;

public class OutOfTimeKill : MonoBehaviour {
    public float t;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, t);
	}
}
