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
	public AudioSource audioSource;
	bool fadeOutAudio;

	public int currentStageNum;

	void Awake()
	{
		Screen.SetResolution(1920, 1080, true);
        PlayerManager.hp = 100;

    }
	void Start()
	{
		if(isTrigger)
		{
			Invoke("LoadSceneTrigger", changeTime);
		}
	}

	void Update()
	{
		if (!fadeOutAudio) return;
		if (audioSource.volume >= 0)
		{
			audioSource.volume -= Time.deltaTime * 0.25f;
		}
		else
		{
			audioSource.Stop();
			audioSource.volume = 1f;
			fadeOutAudio = false;
		}
	}

	public void LoadSceneTrigger()
	{
		SceneManager.LoadScene(nextSceneName);
	}

	public IEnumerator LoadSceneAfterTime(float time)
	{
		fadeOutAudio = true;
		LoadLoadingScene();
		Invoke("LoadSceneTrigger", time);
		yield return null;
	}

	void LoadLoadingScene()
	{
		nextSceneName = loadingNames[Random.Range(0, loadingNames.Length)];
	}
}
