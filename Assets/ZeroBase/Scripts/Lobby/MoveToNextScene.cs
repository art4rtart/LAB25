using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextScene : MonoBehaviour
{
	public string nextSceneName;
	public bool isTrigger;
	public float changeTime;
	string[] loadingNames = { "Loading1", "Loading2", "Loading3", "Loading4", "Loading5" };

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

	public IEnumerator LoadSceneAfterTime(float time)
	{
		LoadLoadingScene();
		Invoke("LoadSceneTrigger", time);
		yield return null;
	}

	void LoadLoadingScene()
	{
		nextSceneName = loadingNames[Random.Range(0, loadingNames.Length)];
	}
}
