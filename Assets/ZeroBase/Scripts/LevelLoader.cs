using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
	public string sceneName;
	public Slider slider;

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

			slider.value = progress;

			yield return null;
		}
	}
}
