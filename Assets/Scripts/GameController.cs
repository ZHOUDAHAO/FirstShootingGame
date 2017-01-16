using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class Round
{
    public Round(GameObject roundBoss, GameObject[] roundEnemies, int[] roundCount, int roundKinds)
    {
        this.roundBoss = roundBoss;
        this.roundEnemies = roundEnemies;
        this.roundCount = roundCount;
        this.roundKinds = roundKinds;
    }
    public GameObject roundBoss;
    public GameObject[] roundEnemies;
    public int[] roundCount;
    public int roundKinds;//每一轮的怪物种类
}
public class GameController : MonoBehaviour {
    public GameObject enemy_green;
    public GameObject enemy_blue;
    public GameObject enemy;
    public GameObject boss;
    public Round[] round;

    public Vector3 bornPosition;
    public Vector3 bossPosition;

    public float singleEnemyWait;
    public float littleRoundWait;   
    public float bigRoundWait;

    public GUIText pointText;
    private int totalPoint;
    public GUIText gameOverText;
    public GUIText restartText;
    public GUIText roundText;

    private bool gameOver;
    private bool restart;

    public int currentRound = 0;//记录当前的轮次
    public bool startRound = true;

    private int[][] roundEnemyCount;
    public int roundIncrement;//每一个大轮之后怪物数量的增量

    public int[] enemyHealth;
    public int increment1, increment2, increment3, increment4;
    void init_roundEnemyCount()
    {
        roundEnemyCount = new int[7][];
        roundEnemyCount[0] = new int[] { 3 };
        roundEnemyCount[1] = new int[] { 5 };
        roundEnemyCount[2] = new int[] { 6 };

        roundEnemyCount[3] = new int[] { 3, 6 };
        roundEnemyCount[4] = new int[] { 4, 7 };
        roundEnemyCount[5] = new int[] { 5, 8 };

        roundEnemyCount[6] = new int[] { 3, 6, 8 };
    }
    void init_round()
    {
        round = new Round[7];
        round[0] = new Round(enemy_blue, new GameObject[] { enemy_green }, roundEnemyCount[0], 1);
        round[1] = new Round(enemy, new GameObject[] { enemy_blue }, roundEnemyCount[1], 1);
        round[2] = new Round(boss, new GameObject[] { enemy }, roundEnemyCount[2], 1);

        round[3] = new Round(enemy, new GameObject[] { enemy_green, enemy_blue }, roundEnemyCount[3], 2);
        round[4] = new Round(boss, new GameObject[] { enemy_green, enemy }, roundEnemyCount[4], 2);
        round[5] = new Round(boss, new GameObject[] { enemy_blue, enemy }, roundEnemyCount[5], 2);

        round[6] = new Round(boss, new GameObject[] { enemy_green, enemy_blue, enemy }, roundEnemyCount[6], 3);
    }

    public void create_NewEnemy()
    {
        int i = currentRound % 7;
        for (int j = 0; j < roundEnemyCount[i].Length; j++)
        {
            roundEnemyCount[i][j] += roundIncrement;
        }
        if (i == 0 && currentRound > 0)//加血量
        {
            enemyHealth[0] += increment1;
            enemyHealth[1] += increment2;
            enemyHealth[2] += increment3;
            enemyHealth[3] += increment4;
        }
        StartCoroutine(create_RoundEnemy(currentRound));
        currentRound++;
    }
    public static GameController get_gameController()
    {
        GameController gmCtrl;
        GameObject gmCtrlObj = GameObject.FindWithTag("GameController");
        gmCtrl = gmCtrlObj.GetComponent<GameController>();
        return gmCtrl;
    }
    void Start()
    {
        enemyHealth = new int[4] { 2, 3, 5, 20 };
        GetComponent<AudioSource>().Play();
        gameOver = restart = false;
        gameOverText.text = "";
        restartText.text = "";
        roundText.text = "Round " + currentRound;
        totalPoint = 0;
        UpdatePoint();
        init_roundEnemyCount();
        init_round();
        create_NewEnemy();
    }

    //roundEnemies数组元素的个数是非boss飞船的个数,数组元素都不是关卡boss
    public IEnumerator create_RoundEnemy(int currentRound)
    {
        Vector3 position;
        GameObject roundBoss;
        GameObject[] roundEnemies;
        int[] enemyCount;
        int enemyKinds;
        roundText.text = "Round " + currentRound;

        yield return new WaitForSeconds(bigRoundWait);
        roundText.text = "";
        int i = currentRound % 7;
        //以下的代码是一个大轮的代码 
        roundBoss = round[i].roundBoss;
        roundEnemies = round[i].roundEnemies;
        enemyCount = round[i].roundCount;
        enemyKinds = round[i].roundKinds;
        //以下的代码是一个小轮的代码
        for (int j = 0; j < enemyKinds; j++)
        {
            for (int k = 0; k < enemyCount[j]; k++)
            {
                position = new Vector3(
                    UnityEngine.Random.Range(-bornPosition.x, bornPosition.x),
                    bornPosition.y,
                    bornPosition.z
                    );
                if (gameOver == false)
                    Instantiate(roundEnemies[j], position, roundEnemies[j].transform.rotation);
                else
                {
                    restartText.text = "Press R to restart";
                    restart = true;
                }
                yield return new WaitForSeconds(singleEnemyWait);
            }
            yield return new WaitForSeconds(littleRoundWait);
        }
        //处理Boss
        position = new Vector3(0, bornPosition.y, bornPosition.z);
        UnityEngine.Object newBoss = Instantiate(roundBoss, position, roundBoss.transform.rotation);
        newBoss.name = "Boss";
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        } 
    }

    void UpdatePoint()
    {
        pointText.text = "Point: " + totalPoint;
    }
    public void AddPoint(int point)
    {
        totalPoint += point;
        UpdatePoint();
    }
    public void GameOver()
    {
        roundText.text = "";
        gameOverText.text = "Game Over";
        gameOver = true;
    }
}
