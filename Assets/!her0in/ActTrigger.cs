using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActTrigger : MonoBehaviour
{
	public InfecteeGenerator generator;
	public ScriptController scriptController;
	public MissionScripts missionScript;

	[Header("Generate/Script/Finish")]
	public string triggerType;

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			if(triggerType == "Generate") StartCoroutine(generator.Generate());
			else if (triggerType == "Script") scriptController.typeNextScript = true;
			else if (triggerType == "Finish") missionScript.GetComponent<Animator>().SetTrigger("Finish");
			this.gameObject.SetActive(false);
		}
	}
}
