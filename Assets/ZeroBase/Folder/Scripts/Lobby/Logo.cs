using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
	public string nextSceneName;

	public void LoadSceneTrigger()
	{
		SceneManager.LoadScene(nextSceneName);
	}
}
