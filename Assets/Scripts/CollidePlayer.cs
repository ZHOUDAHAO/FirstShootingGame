using UnityEngine;
using System.Collections;

public class CollidePlayer : MonoBehaviour {
    public GameObject enemyExplosion;
    public GameObject playerExplosion;
    public GameObject healthValue;

    public int playerDiePoint;//玩家死亡之后的补偿分数
    public int health;

    private GameController gameController;
    private AudioSource audioSource;
    private Object[] healthValueObjects;
    private int healthMax = 10;

    ZSH zsh;

    public bool super;
    public float lastSuperTime;
    private Color originColor;
    private Renderer rend;

    public int greenGemUpdate;
    public int redGemUpdate;
    public int blueGemUpdate;
    public int bossGemUpdate;
    void Start()
    {
        super = false;
        zsh = GetComponent<ZSH>();
        rend = zsh.GetComponent<Renderer>();
        originColor = rend.material.color;
        healthValueObjects = new Object[healthMax];
        for (int i = 0; i < health; i++) {
            healthValueObjects[i] = Instantiate(healthValue, new Vector3(-3.9f , 0, 8 - 0.3f * i), healthValue.transform.rotation);
        }
        audioSource = GetComponent<AudioSource>();
        gameController = GameController.get_gameController();
    }
    IEnumerator DestroyExplosion(Object obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }

    void OnTriggerEnter(Collider other)
    {     
        switch (other.tag)
        {
            case "Boundary": return;
            case "BossGemMesh":                
                audioSource.Play();
                zsh.strongShotCount++;
                zsh.strongShotText.text = "" + zsh.strongShotCount;
                Destroy(other.gameObject);
                zsh.strongShotText.text = "" + zsh.strongShotCount;
                break;
            case "Gem":
                GemType gemType = other.gameObject.GetComponent<GemType>();        
                audioSource.Play();
                Destroy(other.gameObject);
                switch (gemType.type)
                {
                    case 0:
                        zsh.blueGemCount++;
                        if (zsh.blueGemCount >= blueGemUpdate)
                        {
                            zsh.blueGemCount -= blueGemUpdate;
                            if (zsh.multiShot == false)
                            {
                                zsh.firstMultiShotTime = Time.time;
                                zsh.multiShot = true;
                            }
                            zsh.lastMultiShotTime = Time.time;
                        }
                        zsh.blueGemText.text = "" + zsh.blueGemCount;
                        break;
                    case 2:
                        zsh.greenGemCount++;
                        if (zsh.greenGemCount >= greenGemUpdate)
                        {
                            zsh.greenGemCount -= greenGemUpdate;
                            super = true;
                            lastSuperTime = Time.time;
                        }
                        zsh.greenGemText.text = "" + zsh.greenGemCount;
                        break;
                    case 3:
                        zsh.redGemCount++;
                        if (zsh.redGemCount >= redGemUpdate)
                        {
                            zsh.redGemCount -= redGemUpdate;
                            if (health < 10)
                            {
                                healthValueObjects[health] = Instantiate(healthValue, new Vector3(-3.9f, 0, 8f - 0.3f * health), healthValue.transform.rotation);
                                health++;
                            }
                        }
                        zsh.redGemText.text = "" + zsh.redGemCount;
                        break;
                    default:
                        Debug.Log(gemType.type);
                        break;
                }
                break;
            case "Boss":
            case "Enemy":
                if (!super)//玩家不在无敌状态
                {
                    Instantiate(playerExplosion, transform.position, transform.rotation);
                    gameController.GameOver();
                    Destroy(gameObject);
                }                                
                Instantiate(enemyExplosion, other.transform.position, other.transform.rotation);             
                gameController.AddPoint(playerDiePoint);               
                Destroy(other.gameObject);
                break;
            case "BoltEnemy":
                if (!super)
                {
                    health--;
                    if (health <= 0)
                    {
                        if (health == 0)
                            Destroy(healthValueObjects[health]);
                        Object obj = Instantiate(playerExplosion, transform.position, transform.rotation);
                        StartCoroutine(DestroyExplosion(obj, 2f));
                        gameController.GameOver();
                        Destroy(gameObject);
                    }
                    else
                    {
                        Destroy(healthValueObjects[health]);
                        Object obj = Instantiate(playerExplosion, transform.position, transform.rotation);
                        StartCoroutine(DestroyExplosion(obj, 0.05f));
                    }
                }
                Destroy(other.gameObject);
                break;
            default:break;              
        }
    }

    void Update()
    {
        if (super)
        {
            rend.material.color = Color.green;
        }
        if (Time.time >= lastSuperTime + 10f)
        {
            rend.material.color = originColor;
            super = false;
        }
    }
}
