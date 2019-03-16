using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UniverseController : MonoBehaviour
{
	public string sceneName;
	public string LimpidName;
	public GameObject[] limpidsA;
	public GameObject[] limpidsB;

	public List<GameObject> temp1;
	public List<GameObject> temp2;

	public List<GameObject> Limpid;

	public bool trigger;

	void Awake()
	{
		// SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
	}

	void Start()
    {
		StartCoroutine(LoadAsynchronously(sceneName));
	}

	IEnumerator LoadAsynchronously(string sceneName)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / .9f);
			yield return null;
		}
		LoadLimpidData();
	}

	void LoadLimpidData()
	{
		trigger = true;
		limpidsA = GameObject.FindGameObjectsWithTag("LimpidA");
		limpidsB = GameObject.FindGameObjectsWithTag("LimpidB");

		// 리스트에 넣어 정렬
		for (int i = 0; i < limpidsA.Length; i++)
		{
			for (int j = 0; j < limpidsA.Length; j++)
			{
				if (limpidsA[j].transform.name == LimpidName + " (" + (i + 1) + ")")
				{
					temp1.Add(limpidsA[j]);
					break;
				}
			}
		}

		for (int i = 0; i < limpidsA.Length; i++)
		{
			for (int j = 0; j < limpidsA.Length; j++)
			{
				if (limpidsB[j].transform.name == LimpidName + " (" + (i + 1) + ")")
				{
					temp2.Add(limpidsB[j]);
					break;
				}
			}
		}

		for (int i = 0; i < temp1.Count; i++)
		{
			Limpid.Add(temp1[i]);
			Limpid.Add(temp2[i]);
		}

		// 자식 객체에 붙여 주는 것
		for (int i = 0; i < temp1.Count; i++)
		{
			for (int j = 0; j < temp2.Count; j++)
			{
				if (temp2[j].transform.gameObject.name == temp1[i].transform.gameObject.name)
				{
					temp2[j].transform.SetParent(temp1[i].transform);
					break;
				}
			}
		}

		for (int i = 0; i < temp1.Count; i++)
		{
			for (int j = 0; j < temp2.Count; j++)
			{
				if (temp2[j].transform.gameObject.name == temp1[i].transform.gameObject.name)
				{
					temp2[j].transform.SetParent(temp1[i].transform);
					break;
				}
			}
		}

		for (int i = 0; i < temp1.Count; i++)
		{
			Animator anim = temp1[i].GetComponent<Animator>();
			temp2[i].transform.GetComponent<Animator>().runtimeAnimatorController = anim.runtimeAnimatorController;
		}
	}
}
