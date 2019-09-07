using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateZombie : MonoBehaviour
{
	public LabAgent labAgent;
	public GameObject[] Zombies;
	int zombieIndex = 0;

	void Awake()
	{
		ActivateZombie();
	}

	public void ActivateZombie()
	{
		for(int i = 0;i<Zombies.Length; i++) Zombies[i].SetActive(false);
		zombieIndex = Random.Range(0, Zombies.Length);
		Zombies[zombieIndex].SetActive(true);
		labAgent.target = Zombies[zombieIndex];
		Zombies[zombieIndex].GetComponent<Feature>().GenerateFeatures();
	}
}
