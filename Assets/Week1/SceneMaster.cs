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
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) Escape();
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
		float alpha = 0;
		fadeInImage.enabled = true;

		while(alpha <= 1f)
		{
			fadeInImage.color = new Color(fadeInImage.color.r, fadeInImage.color.g, fadeInImage.color.b, alpha);
			alpha += Time.deltaTime * 2f;
			yield return null;
		}
		SceneManager.LoadScene(sceneName);

		yield return new WaitForSeconds(2f);			// scene loading time
		fadeInImage.enabled = false;

		StopAllCoroutines();
	}
}
