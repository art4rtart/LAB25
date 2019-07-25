using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeSentence : MonoBehaviour
{
	public TextMeshProUGUI text;
	public float speed;
	public string sentence;
	public bool isStarted;
	public bool justTypeIt;

	void Start()
	{
		if(justTypeIt) StartCoroutine(Type(text.text));
		else this.enabled = false;
	}

	public IEnumerator Type(string sentence)
	{
		isStarted = true;
		text.text = "";

		for (int i = 0; i < sentence.Length; i++)
		{
			text.text += sentence[i];
			yield return new WaitForSeconds(speed);
		}
	}
}
