using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System.Reflection;
using UnityEditor;
public class LabAgent : Agent
{

	//[Header("Target Featuer Data")]
	//public string targetName = "Who is it?";
	//public string targetBehavior = "What is it doing?";
	//string[] targetNames = new string[] { "Human", "Zombie_1", "Zombie_2", "Transparent", "Police", "Boss" };
	//string[] targetBehaviors = new string[] { "Idle_1", "Idle_2", "Idle_3", "Walk_1", "Walk_2", "Walk_3", "Run_1", "Run_2", "Attack_1", "Attack_2", "Attack_3", "Attack_4", "Attack_5", "Crash", "Die", "Shoot", "Push" };

	public Feature feature;
	public AgentWeaponCtrl agentWeaponCtrl;
    //public GeneratorML generator;
    public GameObject target;

	public bool startObservation;
	public bool foundWho = false;
    public bool foundWhat = false;

    public enum AGENTACTION { NONE, GUN, SHOTGUN, FLAME };
    public AGENTACTION curAction;

    public int guessTargetID = -1;
    private int targetID = -1;

    public bool isShot = false;
    public int damage = 0;

    //private int cnt; 

    //Debug Variable
    private int cnt = 0;
    private int curscore = 0;
    private int cnt1 = 0;
    private int cnt2 = 0;
    private int cnt3 = 0;
    private int cnt4 = 0;
    private int cnt5 = 0;
    private int cnt6 = 0;

    private int missShot = 0;

    public override void InitializeAgent()
	{
		//StartCoroutine(ClearConsole());
		base.InitializeAgent();
	}

	public override void CollectObservations()
	{

		if (feature == null || target == null)
		{
            //Debug.Log("ASD");
			//this.enabled = false;
		}

		if (feature == null)
		{
			for (int i = 0; i < 20; i++)
				AddVectorObs(0f);
		}
		else
		{

            AddVectorObs((int)feature.myEyeColor);
            AddVectorObs((int)feature.myBody);
            AddVectorObs((int)feature.myBrain);
            AddVectorObs((int)feature.myCloth);
            AddVectorObs((int)feature.wkPower);
            AddVectorObs((int)feature.rnPower);
            AddVectorObs((int)feature.akTendency);
            AddVectorObs((int)feature.psDgr);

            AddVectorObs(feature.height / Feature.MAX_HEIGHT);
            AddVectorObs(feature.weight / Feature.MAX_WEIGHT);
            //AddVectorObs(feature.senseOfSight);
            //AddVectorObs(feature.senseOfSmell);
            //AddVectorObs(feature.senseOfHearing);
            //AddVectorObs(feature.senseOfTouch);
            //AddVectorObs(feature.senseOfTaste);
            //AddVectorObs(feature.RecognizeRange / Feature.MAX_RECOGNIZE_RANGE);
            
            if (guessTargetID != -1)
                AddVectorObs((guessTargetID == targetID) ? 1.0f : 0f);
            else
                AddVectorObs(0f);
            AddVectorObs(foundWho);

            //AddVectorObs(guessTargetID == target.GetComponent<Feature>().infecteeID ? 1.0f : 0f);
            for (int action = 0; action <= (int)AGENTACTION.FLAME; ++action)
                AddVectorObs((int)curAction == action ? 1.0f : 0.0f);
            //AddVectorObs(isShot);
            AddVectorObs(guessTargetID);
            AddVectorObs(damage);
            AddVectorObs(target.GetComponent<Health>().hp/100);

            //AddVectorObs(generator.killedInfectee /  generator.generatedZombieCount);
            AddVectorObs(0);

            //for (int i = 0; i < feature.Ankles.Length; i++)
            //{
            //	AddVectorObs(feature.Ankles[i].localPosition);
            //	AddVectorObs(feature.Ankles[i].localRotation);
            //}
        }
	}

    public void MoveAgent(float[] act)
    {
        //Debug.Log(++cnt);
       
        //foundWho = false;
        if (feature == null) return;
        AddReward(-0.5f);
        damage = 0;
        int who = Mathf.FloorToInt(act[0]);
        int how = Mathf.FloorToInt(act[1]);
        //int how = Mathf.FloorToInt(act[1]);
        targetID = target.GetComponent<Feature>().infecteeID;
        if (!foundWho)
        {
            switch (who)
            {
                case 1:
                    guessTargetID = 0;
                    break;
                case 2:
                    guessTargetID = 1;
                    break;
                case 3:
                    guessTargetID = 2;
                    break;
                case 4:
                    guessTargetID = 3;
                    break;
                case 5:
                    guessTargetID = 4;
                    break;
                case 6:
                    guessTargetID = 5;
                    break;
            }

            if (guessTargetID.Equals(targetID) && targetID != -1)
                foundWho = true;
        }
        if( foundWho)
        {
            curAction = AGENTACTION.NONE;
            switch (how)
            {
                case 0:
                    agentWeaponCtrl.flameThrower.StopFlameThrower();
                    //AddReward(-0.1f);
                    break;
                case 1:
                    // gun
                    agentWeaponCtrl.flameThrower.StopFlameThrower();
                    agentWeaponCtrl.Fire();
                    
                    break;

                case 2:
                    // shotGun
                    agentWeaponCtrl.flameThrower.StopFlameThrower();
                    agentWeaponCtrl.FireShotGun();
                    //curAction = AGENTACTION.SHOTGUN;
                    break;

                case 3:
                    // flame thrower
                    agentWeaponCtrl.FireFlameThrower();
                    //curAction = AGENTACTION.FLAME;
                    break;
            }
        }
       
        if (guessTargetID.Equals(targetID) && targetID != -1)
        {
            //Debug.Log("Clear " + target.transform.parent.name);
            //Debug.Log("CLEAR");
            foundWho = true;
            AddReward(0.1f);

            curscore++;
            switch (guessTargetID)
            {

                case 0:
                case 1:
                    if (!curAction.Equals(AGENTACTION.GUN))
                    {
                        missShot++;
                        Debug.Log("Miss Gun");
                    }
                    break;
                case 2:
                case 3:
                    if (!curAction.Equals(AGENTACTION.SHOTGUN))
                    {
                        missShot++;
                        Debug.Log("Miss Shot");
                    }
                    break;
                case 4:
                case 5:
                    if (!curAction.Equals(AGENTACTION.FLAME))
                    {
                        missShot++;
                        Debug.Log("Miss Flame");
                    }
                    break;
            }

            Debug.Log(guessTargetID + "  " + curAction);
            if (curscore % 100 == 0)
                Debug.Log(missShot / (float)(curscore) + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            //Debug.Log(target);
            //Debug.Log(target.transform.parent);
            //generator.enemyPool[target.transform.parent.GetComponent<ChangeRagdollML>().InfecteeID].RemoveItem(target.transform.parent.gameObject, null, generator.transform);

            //generator.killedInfectee++;
            //cnt++;
            //curscore++;
            //if( cnt >= 10000 )
            //{
            //    Debug.Log("Result = " + (float)curscore / cnt + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            //    Debug.Log(cnt1 + " " + cnt2 + " " + cnt3 + " " + cnt4 + " " + cnt5 + " " + cnt6);
            //    EditorApplication.isPaused = true;
            //}


            //if (generator.killedInfectee == generator.generatedZombieCount)
            //    Done();
            //else
            //    ResetValues();
        }
        else
        {
            foundWho = false;
            //switch(targetID)
            //{
            //    case 0:
            //        cnt1++;
            //        break;
            //    case 1:
            //        cnt2++;
            //        break;
            //    case 2:
            //        cnt3++;
            //        break;
            //    case 3:
            //        cnt4++;
            //        break;
            //    case 4:
            //        cnt5++;
            //        break;
            //    case 5:
            //        cnt6++;
            //        break;
            //}
            //cnt++;
            //Debug.Log("Fail" + guessTargetID + "  " + target.transform.parent.name );
            AddReward(-0.5f);
        }
    }


	//void FindOtherTargetReward()
	//{
	//	finding = true;
	//	if (finding && !findingOKTemp)
	//	{
	//		if (target.GetComponent<Feature>().distance >= recognizeDistance)
	//		{
	//			AddReward(.5f);
	//			Done();
	//		}

	//		else if (target.GetComponent<Feature>().distance < recognizeDistance)
	//		{
	//			AddReward(-10f);
	//			Done();
	//		}
	//		rescuingOKTemp = true;
	//	}
	//}

	//void RescueReward()
	//{
	//	rescuing = true;
	//	if (rescuing && !rescuingOKTemp)
	//	{
	//		if (target.GetComponent<Feature>().nameHash == 1)
	//		{
	//			AddReward(1f);
	//			//Debug.Log("Good, I Rescued Human");
	//			Done();
	//		}

	//		else if (target.GetComponent<Feature>().nameHash != 1)
	//		{
	//			AddReward(-5f);
	//			//Debug.Log("Bad (Rescue)");
	//			Done();
	//		}
	//		rescuingOKTemp = true;
	//	}
	//}

	//void DistanceReward()
	//{
	//	calculatingDistance = true;
	//	if (calculatingDistance && !calculatingOKTemp)
	//	{
	//		if (target.GetComponent<Feature>().distance < recognizeDistance)
	//		{
	//			AddReward(1f);
	//			// Debug.Log("Good");
	//		}

	//		else
	//		{
	//			AddReward(-5f);
	//			//Debug.Log("Bad");
	//		}
	//		calculatingOKTemp = true;
	//	}
	//}

	public override void AgentAction(float[] vectorAction, string textAction)
	{
		if (!startObservation) return;

		MoveAgent(vectorAction);

		AddReward(-1f / agentParameters.maxStep);
	}

    public override void AgentReset()
    {
        //Debug.Log("AgentReset");
        //cnt = 0;
        target = null; feature = null;

        foundWho = false;
        guessTargetID = -1;
        targetID = -1;
        isShot = false;
        agentWeaponCtrl.flameThrower.StopFlameThrower();


        //generator.ResetGenerator();
        //generator.Generate();
        //Debug.Log("ASD");
        //startObservation = false;
        //generateZombie.ActivateZombie();
        //printData = false;
    }
    //public void ResetGenerator()
    //{
    //    generator.ResetGenerator();
    //    generator.Generate();
    //}

    public void ResetValues()
    {
        target = null;
        feature = null;
        foundWho = false;
        //Debug.Log("RESET");
        //transform.position = new Vector3(0, 0, 0);
        guessTargetID = -1;
        targetID = -1;
        isShot = false;
        agentWeaponCtrl.flameThrower.StopFlameThrower();
        //generator.ResetGenerator();
        //startObservation = false;
        //generateZombie.ActivateZombie();
        //printData = false;
    }


    //bool printData;
    //void PrintTargetInfo()
    //{
    //	if (!printData)
    //	{
    //		//Debug.Log("Target is " + target.transform.name + " and it is " + target.GetComponent<Feature>().animationName);
    //		printData = true;
    //	}
    //}

    //IEnumerator ClearConsole()
    //{
    //	while (true)
    //	{
    //		ClearLog();
    //		yield return new WaitForSeconds(30f);
    //	}
    //}

    //public void ClearLog()
    //{
    //	var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
    //	var type = assembly.GetType("UnityEditor.LogEntries");
    //	var method = type.GetMethod("Clear");
    //	method.Invoke(new object(), null);
    //}
}
