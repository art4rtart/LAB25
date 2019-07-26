using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfecteeGenerator : MonoBehaviour
{
	[Header("Spawn Settings")]
	public GameObject[] infectees;
    public GameObject[] spawnZone;
    private Transform parent;

	[Header("Generate Settings")]
	public string generatorName = "Generator";
	public int GenerateTotal;
	public int generate;
    public float generateTime;
	float generatedZombieCount;

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
			enemyPool[i].Create(infectees[i], GenerateTotal, this.transform);
		}
    }

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(Generate());
		}
	}

    void OnApplicationQuit()
    {
		for(int i = 0; i < infectees.Length; i++)
		{
			enemyPool[i].Dispose();
		}
    }

    public IEnumerator Generate()
    {
        GameObject infectee;

        for (int i = 0; i < spawnZone.Length; i++)
        {
			if (spawnZone[i].activeSelf == true)
			{
				for (int j = 0; j < generate; ++j)
				{
					infectee = enemyPool[Random.Range(0, infectees.Length)].NewItem();

					Vector3 pos = spawnZone[i].transform.position + new Vector3(Random.Range(-3.0f, 3.0f), 0, Random.Range(-3.0f, 3.0f));

					if ( infectee != null ) infectee.transform.GetChild(0).position = pos;

					generatedZombieCount++;

					if (generatedZombieCount == GenerateTotal)
					{
						for(int k = 0; k < spawnZone.Length; k++)
						{
							spawnZone[k].SetActive(false);
						}
					}
				}
			}
        }

        yield return new WaitForSeconds(generateTime);

        StartCoroutine(Generate());
    }
}
