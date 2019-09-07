using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToLogoScene : MonoBehaviour
{
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Semicolon))
		{
			Debug.Log("HI");
			SceneManager.LoadScene("1. KPU");
		}
	}
}
