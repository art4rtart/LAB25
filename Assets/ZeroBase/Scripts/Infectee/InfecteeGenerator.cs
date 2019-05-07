using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfecteeGenerator : MonoBehaviour
{
    public GameObject enemy;
    public GameObject enemy2;
    public GameObject enemy3;

    public GameObject[] stage_EnemyZone;
    private Transform parent;
    public int generateNum;
    public int generate;
    public float generateTime;

    public string generatorName = "Generator";

    //ObjectPool
    public MemoryPool enemyPool = new MemoryPool();
    public MemoryPool enemyPool2 = new MemoryPool();
    public MemoryPool enemyPool3 = new MemoryPool();

    //private bool asd = false;

    private void Awake()
    {
        parent = GameObject.Find(generatorName).transform;
    }
    void Start()
    {
        enemyPool.Create(enemy, generateNum, this.transform);
        enemyPool2.Create(enemy2, generateNum, this.transform);
        enemyPool3.Create(enemy3, generateNum, this.transform);

        StartCoroutine(Generate());
        //DontDestroyOnLoad(gameObject);
    }

    //private void Update() // 트리거용
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        if (asd) asd = false;
    //        else
    //        {
    //            asd = true;
    //        }
    //    }
    //}
    void OnApplicationQuit()
    {
        enemyPool.Dispose();
        enemyPool2.Dispose();
        enemyPool3.Dispose();
    }

    IEnumerator Generate()
    {
        GameObject infectee;
        for (int i = 0; i < stage_EnemyZone.Length; i++)
        {
            if (stage_EnemyZone[i].activeSelf == true)
            {
                for (int j = 0; j < generate; ++j)
                {
                    int random = Random.Range(0,3);

                    if (random == 0)
                        infectee = enemyPool.NewItem();
                    else if (random == 1)
                        infectee = enemyPool2.NewItem();
                    else
                        infectee = enemyPool3.NewItem();


                    if (infectee)
                        infectee.transform.GetChild(0).position = stage_EnemyZone[i].transform.position;
                }
            }
        }


        yield return new WaitForSeconds(generateTime);

        StartCoroutine(Generate());
    }
}
