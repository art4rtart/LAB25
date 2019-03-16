using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	public string sceneName;

	void Start()
	{
		LoadLevel(sceneName);
	}

	public void LoadLevel(string sceneName)
	{
		StartCoroutine(LoadAsynchronously(sceneName));
	}

	public IEnumerator LoadAsynchronously(string sceneName)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / .9f);
			Debug.Log(progress);

			yield return null;
		}
	}
}
