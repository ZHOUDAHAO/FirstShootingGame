using UnityEngine;
using System.Collections;

public class BossMove : MonoBehaviour {
    public int BossSpeed;
    private float startTime;
    private float startZ;
    private float xMax = 3.8f;

	void Start () {
        startTime = Time.time;
        startZ = transform.position.z;
	}
    float smoothMove(float x)
    {
        if (x >= 0f && x <= 2f)
        {
            return x - 1;
        }
        else
        {
            return 3 - x;
        }
    }

	// Update is called once per frame
	void Update () {
        if (transform.position.z > 7)
        {
            transform.position = new Vector3(xMax * smoothMove(Mathf.Repeat(Time.time - startTime, 4f)), 0, startZ - BossSpeed * (Time.time - startTime));
        }
        else
            transform.position = new Vector3(xMax * smoothMove(Mathf.Repeat(Time.time - startTime, 4f)), 0, 7);
    }
}
