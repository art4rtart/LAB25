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

	[Header("Stage4")]
	public GameObject[] bombPlace;
	public BombGage bombGage;
	public bool startGenerator;
	public bool isGeneratorExist;

	void Awake()
	{
		missionScript = GetComponent<MissionScripts>();
		missionAnimator = GetComponent<Animator>();
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
		subText.text = missionScript.subSentences[missionScript.subIndex];
		missionScript.subIndex++;

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
		subText.text = missionScript.subSentences[missionScript.subIndex];
		missionScript.subIndex++;

		yield return new WaitForSeconds(5f);

		bombGage = FindObjectOfType<BombGage>();
		bombGage.stageAnimator.SetBool("FadeIn", true);

		bombPlace[0].transform.position = new Vector3(10 + Random.Range(-5f, 5f), bombPlace[0].transform.position.y, 10 + Random.Range(-5f,5f));
		bombPlace[1].transform.position = new Vector3(-10 + Random.Range(-5f, 5f), bombPlace[0].transform.position.y, 10 + Random.Range(-5f, 5f));
		bombPlace[2].transform.position = new Vector3(-10 + Random.Range(-5f, 5f), bombPlace[0].transform.position.y, -10 + Random.Range(-5f, 5f));
		bombPlace[3].transform.position = new Vector3(10 + Random.Range(-5f, 5f), bombPlace[0].transform.position.y, -10 + Random.Range(-5f, 5f));

		for (int i = 0; i < bombPlace.Length; i++)
		{
			bombPlace[i].SetActive(true);
		}

		yield return new WaitForSeconds(3f);
		missionScript.GetComponent<Animator>().SetTrigger("Finish");

		yield return new WaitForSeconds(7f);
		// play zombie comming sound

		yield return new WaitForSeconds(2f);
		if (BombGage.installedBombCount < 4)
		{
			missionScript.Type();
			subText.text = missionScript.subSentences[missionScript.subIndex];
			missionScript.subIndex++;
		}

		yield return new WaitForSeconds(7f);
		missionScript.GetComponent<Animator>().SetTrigger("Finish");

		generator = FindObjectOfType<InfecteeGenerator>().GetComponent<InfecteeGenerator>();
		StartCoroutine(generator.Generate());
	}
}
