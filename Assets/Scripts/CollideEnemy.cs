using UnityEngine;
using System.Collections;

public class CollideEnemy : MonoBehaviour {
    private int value;
    private GameController gameController;

    public GameObject enemyExplosion;
    private int health;//生命值
    public GameObject[] gem;
    public int enemyType;

    void Start()
    {
        PointValue pointValue = GetComponent<PointValue>();
        value = pointValue.point;
        GameObject gmCtrlObj = GameObject.FindWithTag("GameController");
        gameController = gmCtrlObj.GetComponent<GameController>();
        health = gameController.enemyHealth[enemyType];
    }
    IEnumerator DestroyExplosion(Object obj,float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }
    void createGemByX(float x)
    {
        Vector3 gemBornPosition = new Vector3(x, 0, transform.position.z);
        int i = Random.Range(0, 100);
        if (i < 60)
            Instantiate(gem[3], gemBornPosition, Quaternion.identity);
        else if (i < 90)
            Instantiate(gem[0], gemBornPosition, Quaternion.identity);
        else if (i < 95)
            Instantiate(gem[1], gemBornPosition, Quaternion.identity);
        else
            Instantiate(gem[2], gemBornPosition, Quaternion.identity);
    }
    void createGem(bool oneGem)
    {
        createGemByX(transform.position.x);
        if(oneGem == false)
        {
            createGemByX(transform.position.x - 0.3f);
            createGemByX(transform.position.x + 0.3f);
        }
    }
    void KillEnemy()
    {
        Destroy(gameObject);
        gameController.AddPoint(value);
        Object obj = Instantiate(enemyExplosion, transform.position, Quaternion.identity);
        DestroyExplosion(obj, 2f);
        if (tag == "Boss")
            createGem(false);
        else
            createGem(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary")
        {
            return;
        }       
        else if (other.tag == "Bolt" || other.tag == "StrongBolt")
        {
            if (other.tag == "Bolt")
            {
                Destroy(other.gameObject);
                health--;
            }
            else {
                health = 0;
            }

            if (health <= 0)
            {
                health = 999;
                KillEnemy();
            }
            else
            {
                Object obj = Instantiate(enemyExplosion, transform.position, Quaternion.identity);
                DestroyExplosion(obj, 0.03f);
            }
        }
        else if (other.tag == "Player")
        {
            createGem(true);
        }
    }
    void OnDestroy()
    {
        if (name == "Boss")
        {
            gameController.create_NewEnemy();
        }
    }
}
