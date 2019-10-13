using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
	public static SceneMaster instance = null;
	public static string sceneName;

	public static bool isGoBackAvailable;
	public static bool breakCoroutine = false;
	public Image fadeInImage;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}

		else if (instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		SaveCurrentSceneName();
        StartCoroutine(CheckEscapeScene());
    }

    private IEnumerator CheckEscapeScene()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Escape();

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(CheckEscapeScene());
    }

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Backspace)) Escape();
	}

	public static void SaveCurrentSceneName()
	{
		sceneName = SceneManager.GetActiveScene().name;
	}

	public void Escape()
	{
		StartCoroutine(FadeIn());
	}

	IEnumerator FadeIn()
	{
		fadeInImage.enabled = true;

		for (float alpha = 0; alpha <= 1f; alpha += Time.deltaTime)
		{
			fadeInImage.color = new Color(fadeInImage.color.r, fadeInImage.color.g, fadeInImage.color.b, alpha);
			yield return null;
		}
		fadeInImage.color = new Color(fadeInImage.color.r, fadeInImage.color.g, fadeInImage.color.b, 1);
		yield return new WaitForSeconds(.5f);
		SceneManager.LoadScene(sceneName);

		while(sceneName == SceneManager.GetActiveScene().name) yield return null;
		yield return new WaitForSeconds(.5f);
		fadeInImage.enabled = false;

		StopAllCoroutines();
	}
}
