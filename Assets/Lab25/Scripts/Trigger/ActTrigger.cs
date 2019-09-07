using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActTrigger : MonoBehaviour
{
	[Header("Generate/Script/Finish/ChangeScene/BossPlayerLock")]
	public string triggerType;

	[Header("GenerateTriggerSettings")]
	public InfecteeGenerator generator;

	[Header("ScriptTriggerSettings")]
	public ScriptController scriptController;

	[Header("MissionTriggerSettings")]
	public MissionScripts missionScript;

	[Header("SceneChageTriggerSettings")]
	public Animator sceneFadeAnimator;
	public MoveToNextScene moveToNextScene;
	public string nextSceneName;

	[Header("BossPlayerLockTriggerSettings")]
	public Charger[] charger;
	public GameObject Player;

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			if (triggerType.Equals("Generate")) StartCoroutine(generator.Generate());
			else if (triggerType.Equals("Script")) scriptController.typeNextScript = true;
			else if (triggerType.Equals("Finish")) missionScript.GetComponent<Animator>().SetTrigger("Finish");
			else if (triggerType.Equals("ChangeScene")) ChangeScene();
			else if (triggerType.Equals("BossPlayerLock")) BossLock();
			this.gameObject.SetActive(false);
		}
	}

	void ChangeScene()
	{
		sceneFadeAnimator.SetTrigger("SceneEnd");
		StartCoroutine(moveToNextScene.LoadSceneAfterTime(2f));
		LevelLoader.sceneName = nextSceneName;
	}

	void BossLock()
	{
		for(int i = 0; i < charger.Length; i++)
		{
			charger[i].target = Player.transform;
			charger[i].gameObject.SetActive(true);
		}
	}
}
