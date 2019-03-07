using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	public string currentSceneName;
	public string nextSceneName;
	public static SceneChanger instance = null;

	void Start()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(LoadAsynchronously(nextSceneName));
		}
	}

	public IEnumerator LoadAsynchronously(string name)
	{
		AsyncOperation operatoin = SceneManager.LoadSceneAsync(name);

		while(!operatoin.isDone)
		{
			float progress = Mathf.Clamp01(operatoin.progress / .9f);
			yield return null;
		}
	}
}
