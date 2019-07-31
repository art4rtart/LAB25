using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptController : MonoBehaviour
{
	MissionScripts missionScript;
	Animator missionAnimator;
	InfecteeGenerator generator;
	public bool startGenerator;
	public bool isGeneratorExist;
	void Awake()
	{
		missionScript = GetComponent<MissionScripts>();
		missionAnimator = GetComponent<Animator>();

		if (!isGeneratorExist) return;
		generator = FindObjectOfType<InfecteeGenerator>().GetComponent<InfecteeGenerator>();
	}

	void Start()
	{
		StartCoroutine(ScriptStart());
	}

	IEnumerator ScriptStart()
	{
		yield return new WaitForSeconds(0.5f);
		missionScript.Type();

		while (!startGenerator) yield return new WaitForSeconds(.5f);
		Debug.Log("Start");
		StartCoroutine(generator.Generate());
	}
}
