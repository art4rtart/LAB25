using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	public TextMeshProUGUI[] dialogueText;
	public Queue<string> sentences;
	public float speed  = 0f;

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
		for (int i = 0; i < dialogueText.Length; i++) {
			dialogueText[i].text = "";

			for (int j = 0; j < sentence.Length; j++)
			{
				dialogueText[i].text += sentence[j];
				yield return new WaitForSeconds(speed);
			}
		}
	}
}
