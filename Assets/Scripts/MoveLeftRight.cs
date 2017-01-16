using UnityEngine;
using System.Collections;

public class MoveLeftRight : MonoBehaviour
{
    public float ZSpeed;
    public float XSpeed;

    public float xMax;
    private Rigidbody rgbody;

    private Vector3 startPosition;
    private float startRadian;
    private float startTime;//当这个物体被实例化的时候,系统时间过去了多少
    void Start()
    {
        rgbody = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRadian = Mathf.Asin(startPosition.x / xMax);
        startTime = Time.time;
    }
    
    void FixedUpdate()
    {
        float pastTime = Time.time - startTime;
        rgbody.position = new Vector3(xMax * Mathf.Sin(startRadian + XSpeed * pastTime), 0, startPosition.z + ZSpeed * pastTime);
    }
}
