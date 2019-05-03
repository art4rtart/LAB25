using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfecteeGenerator : MonoBehaviour
{
    public GameObject enemy;
    public GameObject enemy2;
    public GameObject enemy3;

    public GameObject[] stage_EnemyZone;
    public static Transform parent;
    public int generateNum;
    public float generateTime;

	[Header("Names")]
	public string infecteeName = "Infectee";
	public string generatorName = "Generator";
	public string spawnZoneTag = "SpawnZone";

    //ObjectPool
    public static MemoryPool enemyPool = new MemoryPool();
    public static MemoryPool enemyPool2 = new MemoryPool();
    public static MemoryPool enemyPool3 = new MemoryPool();


    private void Awake()
    {
        parent = GameObject.Find(generatorName).transform;
    }
    void Start()
    {
        enemyPool.Create(enemy, generateNum, this.transform);
        enemyPool2.Create(enemy2, generateNum, this.transform);
        enemyPool3.Create(enemy3, generateNum, this.transform);

        stage_EnemyZone = GameObject.FindGameObjectsWithTag(spawnZoneTag);
        StartCoroutine(Generate());
        DontDestroyOnLoad(gameObject);
    }

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
            int random = Random.Range(0, 3);
            if (random == 0)
                infectee = enemyPool.NewItem();
            else if (random == 1)
                infectee = enemyPool2.NewItem();
            else 
                infectee = enemyPool3.NewItem();

            if (infectee)
            {
                infectee.transform.GetChild(0).position = stage_EnemyZone[i].transform.position;
                
                //infectee.transform.SetParent(parent);
            }

        }
        yield return new WaitForSeconds(generateTime);
        StartCoroutine(Generate());
    }
}
