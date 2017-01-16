using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
        Rigidbody rgbody = GetComponent<Rigidbody>();
        rgbody.velocity = transform.forward * speed;        
	}
}
