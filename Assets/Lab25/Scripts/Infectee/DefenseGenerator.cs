using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseGenerator : MonoBehaviour
{
    public static DefenseGenerator Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<DefenseGenerator>();
            return instance;
        }
    }
    private static DefenseGenerator instance;

    [Header("Spawn Settings")]
	public GameObject[] infectees;
    public GameObject[] spawnZone;
    private Transform parent;

	[Header("Generate Settings")]
	public string generatorName = "Generator";
	public int generateTotal;
	public int generate;
    public float generateTime;
	float generatedZombieCount;

    //Wave Manage
    public bool allEnemyIsSpawned = false;     //모든 적이 나왔니?
    public float startDelay = 20f;              //시작할 떄, Generate가 불려지기 전까지 시간
    public float stageClearDelay = 50f;         //스테이지 클리어 후, 다음 Generate가 불려지기 전까지 시간
    public int waveEnemy = 30;                  //현재 Level에 적이 총 몇마리 나오는 지
    public int wave = 1;                        //Wave Level
    //[HideInInspector]
    public int killedInfectee = 0;
    public bool stageClear = false;
    

    public MemoryPool[] enemyPool = new MemoryPool[10];

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

        StartCoroutine(StartGenerate());
        StartCoroutine(CheckStageClear());
    }

    void OnApplicationQuit()
    {
		for(int i = 0; i < infectees.Length; i++)
		{
			enemyPool[i].Dispose();
		}
    }

    public IEnumerator CheckStageClear()
    {
        WaitForSeconds checkTimer = new WaitForSeconds(2.0f);
        while (true)
        {
            if (killedInfectee == waveEnemy * wave && allEnemyIsSpawned)
            {
                stageClear = true;
				UTRSManager.Instance.waitingTime = 20f;
				UTRSManager.Instance.UTRSTimeAnim.SetBool("Generate", false);
				generatedZombieCount = 0;
                killedInfectee = 0;
                stageClear = false;
                wave += 1;
                allEnemyIsSpawned = false;

                StartCoroutine(WaveGenerate());
            }
            yield return checkTimer;
        }
    }

    public IEnumerator Generate()
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

            if (generatedZombieCount == waveEnemy * wave)
            {
                allEnemyIsSpawned = true;
                break;
            }

        }

        yield return new WaitForSeconds(generateTime);

        if (!allEnemyIsSpawned)
            StartCoroutine(Generate());
    }

    public IEnumerator StartGenerate()
    {
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(Generate());
		yield return new WaitForSeconds(1f);
		ZombieScanner.Instance.scanning = true;
	}

    public IEnumerator WaveGenerate()
    {
		yield return new WaitForSeconds(stageClearDelay);
		StartCoroutine(Generate());
		yield return new WaitForSeconds(1f);
		ZombieScanner.Instance.scanning = true;
    }
}
