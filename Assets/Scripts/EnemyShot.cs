using UnityEngine;
using System.Collections;

public class EnemyShot : MonoBehaviour {
    public GameObject shot;
    public Transform shotSpawn;

    public float fireRate;
    public float delay;
    private AudioSource audioSource;

    void Awake () {
        audioSource = GetComponent<AudioSource>();       
	}
    void OnBecameVisible()
    {
        InvokeRepeating("Fire", delay, fireRate);
    }
    void OnBecameInvisible()
    {
        CancelInvoke();
    }
    void Fire()
    {
        if (tag == "Boss")
        {
            float delta = 0.2f;
            Vector3 vecLeft = new Vector3(shotSpawn.position.x - delta, shotSpawn.position.y, shotSpawn.position.z);
            Vector3 vecRight = new Vector3(shotSpawn.position.x + delta, shotSpawn.position.y, shotSpawn.position.z);
            Instantiate(shot, vecLeft, shotSpawn.rotation);
            Instantiate(shot, vecRight, shotSpawn.rotation);
        }
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        if(!audioSource.isPlaying)
            audioSource.Play();
    }
}
