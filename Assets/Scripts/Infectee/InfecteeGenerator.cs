using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfecteeGenerator : MonoBehaviour
{
    public static GameObject enemy;
    public static GameObject[] stage_EnemyZone;
    public static Transform parent;
    public int generateNum;
    public float generateTime;

    private float generateOffset = 1.5f;
    //ObjectPool
    public static MemoryPool enemyPool = new MemoryPool();
 

    private void Awake()
    {
        enemy = GameObject.Find("Infectee");
        parent = GameObject.Find("Generator").transform;
    }
    void Start()
    {
        enemyPool.Create(enemy, generateNum, this.transform);
        stage_EnemyZone = GameObject.FindGameObjectsWithTag("SpawnZone");
        StartCoroutine(Generate());
        DontDestroyOnLoad(gameObject);
    }

    void OnApplicationQuit()
    {
        enemyPool.Dispose();
    }

    IEnumerator Generate()
    {
        GameObject infectee;
        
        //for (int i = 0; i < generateNum; i++)
        //{
        //    infectee = enemyPool.NewItem();
        //    if (infectee)
        //    {
        //        infectee.transform.position = new Vector3(Random.Range(-stage1_EnemyZone[0].rangeX, stage1_EnemyZone[0].rangeX), 1f, Random.Range(-stage1_EnemyZone[0].rangeY, stage1_EnemyZone[0].rangeY)) + stage1_EnemyZone[0].transform.position;
        //        infectee.transform.SetParent(parent);
        //    }
        //}

        if(stage_EnemyZone[0] == null)
            stage_EnemyZone = GameObject.FindGameObjectsWithTag("SpawnZone");

        for (int i = 0; i < stage_EnemyZone.Length; i++)
        {
            InfecteeZone stageEnemyZone = stage_EnemyZone[i].GetComponent<InfecteeZone>();


            infectee = enemyPool.NewItem();

            if (infectee)
            {
                infectee.transform.GetChild(0).position = new Vector3(Random.Range(-stageEnemyZone.rangeX + generateOffset, stageEnemyZone.rangeX - generateOffset), transform.position.y, Random.Range(-stageEnemyZone.rangeZ + generateOffset, stageEnemyZone.rangeZ - generateOffset)) + stageEnemyZone.transform.position + new Vector3(0, 0.4f, 0);
                infectee.transform.SetParent(parent);
            }

        }
        yield return new WaitForSeconds(generateTime);
        StartCoroutine(Generate());
    }
}
