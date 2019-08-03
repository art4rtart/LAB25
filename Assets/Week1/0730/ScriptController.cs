using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScriptController : MonoBehaviour
{
	public int stageNum;
	MissionScripts missionScript;
	Animator missionAnimator;
	InfecteeGenerator generator;

	public bool typeNextScript;
	float waitSecond;

	[Header("Stage3")]
	public TextMeshProUGUI subText;
	int subIndex = 0;

	[Header("Stage4")]
	public bool startGenerator;
	public bool isGeneratorExist;

	void Awake()
	{
		missionScript = GetComponent<MissionScripts>();
		missionAnimator = GetComponent<Animator>();
		subIndex = 0;
		typeNextScript = false;

		if (!isGeneratorExist) return;
	}

	void Start()
	{
		if (stageNum == 3) StartCoroutine(Stage3Script());
		else if (stageNum == 4) StartCoroutine(Stage4Script());
	}

	IEnumerator Stage3Script()
	{
		yield return new WaitForSeconds(0.5f);
		missionScript.Type();
		subText.text = missionScript.subSentences[subIndex];
		subIndex++;

		typeNextScript = false;
		while (!typeNextScript)
		{
			yield return null;
		}

		missionScript.GetComponent<Animator>().SetTrigger("Finish");
		typeNextScript = false;

		while (!typeNextScript)
		{
			yield return null;
		}

		StartCoroutine(Stage3Script());
	}

	bool trig;

	void Update()
	{
		if (PlayerCtrl.flashLightOn && missionScript.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SubScriptFadeIn")
			&& missionScript.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !trig)
		{
			typeNextScript = true;
			trig = true;
		}
	}

	IEnumerator Stage4Script()
	{
		yield return new WaitForSeconds(0.5f);
		missionScript.Type();

		print("1");
		while (!startGenerator) yield return new WaitForSeconds(.5f);
		generator = FindObjectOfType<InfecteeGenerator>().GetComponent<InfecteeGenerator>();
		print(generator);
		StartCoroutine(generator.Generate());
	}
}
