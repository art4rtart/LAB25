using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorML : MonoBehaviour
{
    private static DefenseGenerator instance;

    [Header("Spawn Settings")]
    public GameObject[] infectees;
    public GameObject[] spawnZone;
    private Transform parent;

    [Header("Generate Settings")]
    public string generatorName = "Generator";
    public int generateTotal;
    public int generate;
    //public float generateTime;
    public float generatedZombieCount;

    //[HideInInspector]
    public int killedInfectee = 0;
    public bool GenFlag = false;
    public MemoryPool[] enemyPool = new MemoryPool[10];

    public LabAgent labAgent;
    private void Awake()
    {
        parent = GameObject.Find(generatorName).transform;
    }

    void Start()
    {
        for (int i = 0; i < infectees.Length; i++)
        {
            enemyPool[i] = new MemoryPool();

            enemyPool[i].Create(infectees[i], generateTotal, this.transform);
            infectees[i].SetActive(false);
        }

        //Generate();
        // = true;
        //StartCoroutine(Generate());
        //StartCoroutine(StartGenerate());
        //StartCoroutine(CheckStageClear());
    }

    //private void Update()
    //{
    //    if (generatedZombieCount == killedInfectee )
    //    {
    //        labAgent.Done();
    //        Debug.Log("Update");
    //    }


    //}
    //private void OnEnable()
    //{
    //    if (StartFlag)
    //    {
    //        for (int i = 2; i < transform.childCount; ++i)
    //            enemyPool[transform.GetChild(i).GetComponent<ChangeRagdollML>().InfecteeID].RemoveItem(transform.GetChild(i).gameObject, null, transform);

    //        Generate();
    //    }
    //}
    void OnApplicationQuit()
    {
        for (int i = 0; i < infectees.Length; i++)
        {
            enemyPool[i].Dispose();
        }
    }

    //public IEnumerator CheckStageClear()
    //{
    //    WaitForSeconds checkTimer = new WaitForSeconds(1.0f);
    //    while (true)
    //    {
    //        if (killedInfectee == waveEnemy * wave && allEnemyIsSpawned)
    //        {
    //            stageClear = true;

    //            generatedZombieCount = 0;
    //            killedInfectee = 0;
    //            stageClear = false;
    //            wave += 1;
    //            allEnemyIsSpawned = false;

    //            StartCoroutine(WaveGenerate());
    //        }
    //        yield return checkTimer;
    //    }
    //}


    public void Generate()
    {
        GameObject infectee;

        for (int i = 0; i < spawnZone.Length; i++)
        {
            for (int j = 0; j < generate; ++j)
            {
                infectee = enemyPool[Random.Range(0, infectees.Length)].NewItem();

                Vector3 pos = spawnZone[i].transform.position + new Vector3(Random.Range(-3.0f, 3.0f), 0, Random.Range(-3.0f, 3.0f));

                if (infectee != null)
                {
                    infectee.transform.GetChild(0).position = pos;

                    generatedZombieCount++;
                }
            }

            //if (generatedZombieCount == waveEnemy * wave)
            //{
            //    allEnemyIsSpawned = true;
            //    break;
            //}

        }

        //yield return new WaitForSeconds(generateTime);

        //if (!allEnemyIsSpawned)
        //    StartCoroutine(Generate());
    }

    public void ResetGenerator()
    {
        //Debug.Log("B");
        killedInfectee = 0;
        generatedZombieCount = 0;
        for (int i = 0; i < infectees.Length; ++i)
            enemyPool[i].ClearItem();
      

        //Generate();
    }
    //public IEnumerator StartGenerate()
    //{

    //    yield return new WaitForSeconds(startDelay);

    //    StartCoroutine(Generate());


    //}

    //public IEnumerator WaveGenerate()
    //{
    //    yield return new WaitForSeconds(stageClearDelay);
    //    StartCoroutine(Generate());
    //    yield return new WaitForSeconds(1f);

    //}
}
