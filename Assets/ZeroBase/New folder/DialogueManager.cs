using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	public TextMeshProUGUI dialogueText;

	public Queue<string> sentences;

	void Start()
	{
		sentences = new Queue<string>();
	}

	public void StartDialouge(Dialogue dialogue)
	{
		sentences.Clear();

		foreach(string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if(sentences.Count == 0)
		{
			EndDialouge();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	void EndDialouge()
	{
		Debug.Log("End of conversation");
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";

		for (int i = 0; i < sentence.Length; i++)
		{
			dialogueText.text += sentence[i];
			yield return null;
		}
	}
}
