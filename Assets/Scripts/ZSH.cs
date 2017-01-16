using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class ZSH : MonoBehaviour {
    public float speed;
    public Boundary boundary;
    public float tiltfactor;

    public GameObject shot;
    public Transform shotSpawn;

    public float fireInterval;
    private float nextFire = 0.0f;
    public float strongInterval;
    private float nextStrong = 0.0f;

    public GameObject strongShot;

    public int strongShotCount;
    public int blueGemCount;
    public int redGemCount;
    public int greenGemCount;

    public GUIText strongShotText;
    public GUIText blueGemText;
    public GUIText greenGemText;
    public GUIText redGemText;

    public bool multiShot = false;
    public float lastMultiShotTime = 0f;
    public float firstMultiShotTime;

    public GameObject enemyExplosion;
    private CollidePlayer cp;
    void Start()
    {
        cp = GetComponent<CollidePlayer>();
    }
    void Destroy_all_enemies()
    {
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        if (bosses.Length != 0)
        {
            for(int i = 0; i < bosses.Length; i++)
            {
                Instantiate(enemyExplosion, bosses[i].transform.position, bosses[i].transform.rotation);
                Destroy(bosses[i]);
            }
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length != 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                Instantiate(enemyExplosion, enemies[i].transform.position, enemies[i].transform.rotation);
                Destroy(enemies[i]);
            }
        }
        GameObject[] boltEnemy = GameObject.FindGameObjectsWithTag("BoltEnemy");
        if (boltEnemy.Length != 0)
        {
            for (int i = 0; i < boltEnemy.Length; i++)
            {
                Destroy(boltEnemy[i]);
            }
        }
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireInterval;
            if (!multiShot)
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            else if (Time.time < lastMultiShotTime + 10f)
            {
                float delta = 0.2f;
                Vector3 vecLeft = new Vector3(shotSpawn.position.x - delta, shotSpawn.position.y, shotSpawn.position.z);
                Vector3 vecRight = new Vector3(shotSpawn.position.x + delta, shotSpawn.position.y, shotSpawn.position.z);
                Instantiate(shot, vecLeft, shotSpawn.rotation);
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                Instantiate(shot, vecRight, shotSpawn.rotation);
            }
            else
                multiShot = false;
        }
        if (Input.GetKey(KeyCode.G) && Time.time > nextStrong && strongShotCount >= cp.bossGemUpdate)
        {
            nextStrong = Time.time + strongInterval;
            strongShotCount -= cp.bossGemUpdate;
            strongShotText.text = "" + strongShotCount;
            Instantiate(strongShot, shotSpawn.position, shotSpawn.rotation);
            Destroy_all_enemies();
        }
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Rigidbody rgbody = GetComponent<Rigidbody>();
        
        rgbody.velocity = new Vector3(moveHorizontal, 0.0f, moveVertical) * speed;
        rgbody.position = new Vector3(
            Mathf.Clamp(rgbody.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rgbody.position.z, boundary.zMin, boundary.zMax)
        );
        rgbody.rotation = Quaternion.Euler(0.0f, 0.0f, rgbody.velocity.x * tiltfactor);
    }    
}
