using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	public Dialogue dialogue;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			TriggerDialogue();
		}
	}

	public void TriggerDialogue()
	{
		FindObjectOfType<DialogueManager>().StartDialouge(dialogue);
	}
}
