using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System.Reflection;

public class LabAgent : Agent
{
	[Header("Target Featuer Data")]
	public string targetName = "Who is it?";
	public string targetBehavior = "What is it doing?";
	string[] targetNames = new string[] { "Human", "Zombie_1", "Zombie_2", "Transparent", "Police", "Boss" };
	string[] targetBehaviors = new string[] { "Idle_1", "Idle_2", "Idle_3", "Walk_1", "Walk_2", "Walk_3", "Run_1", "Run_2", "Attack_1", "Attack_2", "Attack_3", "Attack_4", "Attack_5", "Crash", "Die", "Shoot", "Push" };

	[Header("Her0in")]
	public float ShotGunMissed;
	public float normalGunMissed;
	public float flameThrowerMissed;
	public float totalStep;


	public Feature feature;
	public GenerateZombie generateZombie;
	public AgentWeaponCtrl agentWeaponCtrl;
	public GameObject target;

	public bool startObservation;
	bool rescuing;
	bool rescuingOKTemp;

	bool giveReward;
	public bool foundWho = false;
	public bool foundWhat = false;

	public float recognizeDistance = 20f;
	public bool calculatingDistance = false;
	bool calculatingOKTemp;

	public bool isUsingFlame;
	public bool isUsingShotGun;
	public bool isUsingGun;

	public override void InitializeAgent()
	{
		StartCoroutine(ClearConsole());
		base.InitializeAgent();
	}

	public override void CollectObservations()
	{
        if (target != null && !startObservation)
        {
            feature = target.GetComponent<Feature>();
            feature.GetCurrentClipName();

            if (feature.animationName != "NULL" && feature.animationName != "")
            {
                startObservation = true;
				PrintTargetInfo();
            }
        }
       
		AddVectorObs(feature.nameHash);
		AddVectorObs(feature.eyeColor.r);
		AddVectorObs(feature.eyeColor.g);
		AddVectorObs(feature.eyeColor.b);
		AddVectorObs(isUsingGun);
		AddVectorObs(isUsingShotGun);
		AddVectorObs(isUsingFlame);
		AddVectorObs(foundWhat);
        AddVectorObs(foundWho);
		AddVectorObs(target.GetComponent<Health>().hp);

        for (int i = 0; i < feature.Ankles.Length; i++)
        {
            AddVectorObs(feature.Ankles[i].localPosition);
            AddVectorObs(feature.Ankles[i].localRotation);
        }
    }

	public void MoveAgent(float[] act)
	{
        int who = Mathf.FloorToInt(act[0]);
		int what = Mathf.FloorToInt(act[1]);
		int how = Mathf.FloorToInt(act[2]);

		if (!foundWho)
		{
			switch (who)
			{
				case 1:
					targetName = targetNames[0];
					break;
				case 2:
					targetName = targetNames[1];
					break;
				case 3:
					targetName = targetNames[2];
					break;
				case 4:
					targetName = targetNames[3];
					break;
				case 5:
					targetName = targetNames[4];
					break;
				case 6:
					targetName = targetNames[5];
					break;
			}
		}

		if (!foundWhat)
		{
			switch (what)
			{
				case 1:
					targetBehavior = targetBehaviors[0];
					break;
				case 2:
					targetBehavior = targetBehaviors[1];
					break;
				case 3:
					targetBehavior = targetBehaviors[2];
					break;
				case 4:
					targetBehavior = targetBehaviors[3];
					break;
				case 5:
					targetBehavior = targetBehaviors[4];
					break;
				case 6:
					targetBehavior = targetBehaviors[5];
					break;
				case 7:
					targetBehavior = targetBehaviors[6];
					break;
				case 8:
					targetBehavior = targetBehaviors[7];
					break;
				case 9:
					targetBehavior = targetBehaviors[8];
					break;
				case 10:
					targetBehavior = targetBehaviors[9];
					break;
				case 11:
					targetBehavior = targetBehaviors[10];
					break;
				case 12:
					targetBehavior = targetBehaviors[11];
					break;
				case 13:
					targetBehavior = targetBehaviors[12];
					break;
				case 14:
					targetBehavior = targetBehaviors[13];
					break;
				case 15:
					targetBehavior = targetBehaviors[14];
					break;
				case 16:
					targetBehavior = targetBehaviors[15];
					break;
				case 17:
					targetBehavior = targetBehaviors[16];
					break;
			}
		}

		if (foundWhat && foundWho)
		{
			switch (how)
			{
				//case 1:
				//	FindOtherTargetReward();
				//	break;

				case 2:
					RescueReward();
					break;

				case 3:
					agentWeaponCtrl.Fire();
					isUsingGun = true;
					isUsingShotGun = false;
					isUsingFlame = false;
					//DistanceReward();
					break;

				case 4:
					// shotGun
					agentWeaponCtrl.FireShotGun();
					isUsingGun = false;
					isUsingShotGun = true;
					isUsingFlame = false;
					break;

				case 5:
					// flame thrower
					agentWeaponCtrl.FireFlameThrower();
					isUsingGun = false;
					isUsingShotGun = false;
					isUsingFlame = true;
					break;
			}

			// if kill zombie add reward
			if (target.GetComponent<Health>().hp <= 0 && target.GetComponent<Feature>().nameHash != 1) // && feature.distance < recognizeDistance
			{
				AddReward(1f);
				Debug.Log("Good, You Killed Zombie");
				Done();
			}

			//else if (target.GetComponent<Health>().hp <= 0 && target.GetComponent<Feature>().nameHash != 1) //  && feature.distance >= recognizeDistance
			//{
			//	AddReward(-5f);
			//	Debug.Log("Bad");
			//	Done();
			//}

			else if (target.GetComponent<Health>().hp <= 0 && target.GetComponent<Feature>().nameHash == 1)
			{
				AddReward(-5f);
				Debug.Log("Bad (Killed Human)");
				Done();
			}
		}

		if (feature == null) return;
		if (targetName == targetNames[feature.nameHash - 1]) { foundWho = true; }
		if (targetBehavior == feature.animationName) { foundWhat = true; }
		if(foundWho && foundWhat && !giveReward) { AddReward(1f); giveReward = true; }


		episodeTimeCount = Mathf.Clamp(episodeTimeCount += Time.deltaTime,0,episodeTotalTime);
		if (episodeTimeCount >= episodeTotalTime) { AddReward(-5f); AgentReset(); episodeTimeCount = 0; }
	}

	public float episodeTimeCount;
	public float episodeTotalTime = 15f;

	bool finding;
	bool findingOKTemp;

	void FindOtherTargetReward()
	{
		finding = true;
		if (finding && !findingOKTemp)
		{
			if (target.GetComponent<Feature>().distance >= recognizeDistance)
			{
				AddReward(.5f);
				Done();
			}

			else if (target.GetComponent<Feature>().distance < recognizeDistance)
			{
				AddReward(-10f);
				Done();
			}
			rescuingOKTemp = true;
		}
	}

	void RescueReward()
	{
		rescuing = true;
		if (rescuing && !rescuingOKTemp)
		{
			if (target.GetComponent<Feature>().nameHash == 1)
			{
				AddReward(1f);
				Debug.Log("Good, I Rescued Human");
				Done();
			}

			else if (target.GetComponent<Feature>().nameHash != 1)
			{
				AddReward(-5f);
				Debug.Log("Bad (Rescue)");
				Done();
			}
			rescuingOKTemp = true;
		}
	}

	void DistanceReward()
	{
		calculatingDistance = true;
		if (calculatingDistance && !calculatingOKTemp)
		{
			if (target.GetComponent<Feature>().distance < recognizeDistance)
			{
				AddReward(1f);
				// Debug.Log("Good");
			}

			else
			{
				AddReward(-5f);
				Debug.Log("Bad");
			}
			calculatingOKTemp = true;
		}
	}

	public override void AgentAction(float[] vectorAction, string textAction)
	{
		if (!startObservation) return;

		MoveAgent(vectorAction);
	
		AddReward(-1f / agentParameters.maxStep);
	}

	public override void AgentReset()
	{
		target = null; feature = null;

		foundWho = foundWhat = false;
		targetName = targetBehavior = "";
		startObservation = false;
		generateZombie.ActivateZombie();

		rescuing = false;
		rescuingOKTemp = false;
		calculatingDistance = false;
		calculatingOKTemp = false;
		giveReward = false;
		finding = false;
		findingOKTemp = false;

		episodeTimeCount = 0;
		printData = false;
		totalStep++;
	}

	void ResetValues()
	{
		target = null; feature = null;

		foundWho = foundWhat = false;
		targetName = targetBehavior = "";
		startObservation = false;
		generateZombie.ActivateZombie();

		rescuing = false;
		rescuingOKTemp = false;
		calculatingDistance = false;
		calculatingOKTemp = false;
		giveReward = false;
		finding = false;
		findingOKTemp = false;

		episodeTimeCount = 0;
		printData = false;
	}


	bool printData;
	void PrintTargetInfo()
	{
		if (!printData)
		{
			Debug.Log("Target is " + target.transform.name + " and it is " + target.GetComponent<Feature>().animationName);
			printData = true;
		}
	}

	IEnumerator ClearConsole()
	{
		while (true)
		{
			ClearLog();
			yield return new WaitForSeconds(30f);
		}
	}

	public void ClearLog()
	{
		var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
		var type = assembly.GetType("UnityEditor.LogEntries");
		var method = type.GetMethod("Clear");
		method.Invoke(new object(), null);
	}
}
