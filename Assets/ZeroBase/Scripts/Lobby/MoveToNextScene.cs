using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextScene : MonoBehaviour
{
	public string nextSceneName;
	public bool isTrigger;
	public float changeTime;

	void Start()
	{
		if(isTrigger)
		{
			Invoke("LoadSceneTrigger", changeTime);
		}
	}

	public void LoadSceneTrigger()
	{
		SceneManager.LoadScene(nextSceneName);
	}
}
