using UnityEngine;
using System.Collections;

public class BackgroundCtrl: MonoBehaviour {
    public float scrollSpeed;
    public float z;
    private Vector3 startPosition;

	void Start () {
        startPosition = transform.position;
	}
	

	void Update () {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, z);
        transform.position = startPosition + Vector3.forward * newPosition;
	}
}
