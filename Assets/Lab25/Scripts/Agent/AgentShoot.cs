using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AgentShoot : Agent
{
    public Transform[] targets;
    private Rigidbody playerRb;
    public Transform pivotTr;
    private bool isHealing = false;
    private RayPerception rayPer;
    public AgentWeaponCtrl myWeaponCtrl;

    private Transform playerTr;
    private PlayerManager _PlayerManager;

    public static bool isKill = false;
    public static bool isShotMiss = false;

    private Vector3 endPos;
    private int count = 0;
    private bool isFire = false;
    public GameObject shootPos;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        playerRb = GetComponent<Rigidbody>();
        Monitor.verticalOffset = 1f;
        //myWeaponCtrl = GetComponentInChildren<AgentWeaponCtrl>();

        rayPer = GetComponent<RayPerception>();

        //playerTr = GameObject.Find("Player").transform;
        //_PlayerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

    }

    public override void CollectObservations()
    {
        var rayDistance = 1000f;

        //============================ 2-7 =======================
        //float[] rayAngles = { 0f, 15f, 30f, 45f, 60f, 75f, 90f, 105f, 120f, 135f, 150f, 165f, 180f };

        //var detectableObjects = new[] { "Infectee" };
        //AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        //AddVectorObs(shootPoint.forward);
        //AddVectorObs(playerRb.angularVelocity);

        //============================ 2-13 ======================
        //float[] rayAngles = { 0f, 15f, 30f, 45f, 60f, 75f, 90f, 105f, 120f, 135f, 150f, 165f, 180f };

        //var detectableObjects = new[] { "Infectee", "wall", "Player" };

        //AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        //AddVectorObs(shootPoint.forward);
        //AddVectorObs(playerRb.angularVelocity);

        //============================ 2-14 ~ 15 ======================
        //float[] rayAngles = { 0f, 15f, 30f, 45f, 60f, 75f, 90f, 105f, 120f, 135f, 150f, 165f, 180f };

        //var detectableObjects = new[] { "Infectee", "wall", "Player" };

        //endPos = Vector3.Normalize(transform.TransformDirection(
        //          RayPerception.PolarToCartesian(12, 90)));

        //AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        //AddVectorObs(Vector3.Dot(transform.forward, endPos));
        //AddVectorObs(playerRb.angularVelocity);

        //============================ 2-16 ======================
        //float[] rayAngles = { 0, 90f, 180f, 270f, 360f };

        //var detectableObjects = new[] { "Infectee", "wall", "Player" };

        //endPos = Vector3.Normalize(transform.TransformDirection(
        //          RayPerception.PolarToCartesian(12, 90)));

        //AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        //AddVectorObs(Vector3.Dot(transform.forward, endPos));
        //AddVectorObs(playerRb.angularVelocity);

        //============================ 2-17 ======================
        //float[] rayAngles = { 85f, 87f, 90f, 92f, 95f };

        //var detectableObjects = new[] { "Infectee", "wall", "Player" };

        //endPos = Vector3.Normalize(transform.TransformDirection(
        //          RayPerception.PolarToCartesian(12, 90)));

        //AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        //AddVectorObs(Vector3.Dot(transform.forward, endPos));

        //============================ 2-27 ======================
        //float[] rayAngles = { 87f, 90f, 92f };

        //var detectableObjects = new[] { "Infectee", "wall", "Player" };

        //endPos = Vector3.Normalize(transform.TransformDirection(
        //          RayPerception.PolarToCartesian(12, 90)));

        //AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1.5f, 0f));
        //AddVectorObs(Vector3.Dot(transform.forward, endPos));

        //AddVectorObs(playerRb.angularVelocity);
        //AddVectorObs(playerTr.position);

        //============================ 2-28 ======================
        //float[] rayAngles = {45f, 60f, 75f, 90f, 105f, 120f, 135f };

        //var detectableObjects = new[] { "Infectee", "wall", "Player" };

        //endPos = Vector3.Normalize(transform.TransformDirection(
        //          RayPerception.PolarToCartesian(12, 90)));

        //AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        //AddVectorObs(Vector3.Dot(transform.forward, endPos));

        //AddVectorObs(playerRb.angularVelocity);
        //AddVectorObs(playerTr.position);

        //============================ 2-29 ======================
        //float[] rayAngles = { 87f, 90f, 92f };

        //var detectableObjects = new[] { "Infectee", "wall", "Player", "PlayerAgent", "Item", "Untagged",  };

        //endPos = Vector3.Normalize(transform.TransformDirection(
        //          RayPerception.PolarToCartesian(12, 90)));

        //AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1.5f, 0f));
        //AddVectorObs(Vector3.Dot(transform.forward, endPos));

        //======================== 2- 30 ===========================
        //float[] rayAngles = { 90f };

        //var detectableObjects = new[] { "Infectee", "wall", "Player", "PlayerAgent", "Item", "Untagged", "MissionObject"};

        //endPos = Vector3.Normalize(transform.TransformDirection(
        //          RayPerception.PolarToCartesian(12, 90)));

        //AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1.5f, 0f));
        //AddVectorObs(Vector3.Dot(transform.forward, endPos));

        //======================== 2- 31 ===========================
        //float[] rayAngles = { 90f };

        //var detectableObjects = new[] { "Infectee", "wall", "Player", "PlayerAgent", "Item", "Untagged", "MissionObject" };

        //endPos = Vector3.Normalize(transform.TransformDirection(
        //          RayPerception.PolarToCartesian(12, 90)));

        //AddVectorObs(System.Convert.ToInt32(isFire));
        //AddVectorObs(System.Convert.ToInt32(isShotMiss));
        //AddVectorObs(System.Convert.ToInt32(isKill));
        //AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1.5f, 0f));
        //AddVectorObs(Vector3.Dot(transform.forward, endPos));

        //======================== 2 - 32 ===========================
        // 2- 29 25, 2-30 12, 2-31 11, 2-32 13
        float[] rayAngles = { 90f };

        var detectableObjects = new[] { "Infectee", "wall", "Player", "PlayerAgent", "Item", "Untagged", "MissionObject" };

        AddVectorObs(System.Convert.ToInt32(isFire));
        AddVectorObs(System.Convert.ToInt32(isShotMiss));

        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1.0f, 0f));
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        RaycastHit hit;
        GameObject hitObject = null;

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10f;

		if (Physics.Raycast(shootPos.transform.position, forward, out hit, 100f))
		{
			hitObject = hit.transform.gameObject;
		}

		MoveAgent(vectorAction);

        if (hitObject)
        {
            if (hitObject.CompareTag("Infectee") && isFire)
            {
                //Debug.Log("Very Nice");
                AddReward(5.0f / agentParameters.maxStep);
            }
            else if (!hitObject.CompareTag("Infectee") && isFire)
            {
                // Debug.Log("Very Bad");
                AddReward(-5.0f / agentParameters.maxStep);
            }
            else if (hitObject.CompareTag("Infectee") && !isFire)
            {
                // Debug.Log("Bad");
                AddReward(-3.0f / agentParameters.maxStep);
            }
            else
            {
                //Debug.Log("Very Nice2");
                AddReward(5.0f / agentParameters.maxStep);
            }
        }
        else
        {
            if (isFire)
            {
                //Debug.Log("Very Bad");
                AddReward(-5.0f / agentParameters.maxStep);
            }
            else
            {
                //Debug.Log("Very Nice2");
                AddReward(5.0f / agentParameters.maxStep);
            }
        }


        //count++;

        if (isKill)
        {
            //Debug.Log("Nice Kill");
            AddReward(1.0f);
            isKill = false;
        }

        if (isShotMiss)
        {
            AddReward(-5.0f / agentParameters.maxStep);
            isShotMiss = false;
        }


        //if (count == 0)
        //    transform.rotation = Quaternion.identity;
        //else if (count == 100)
        //    transform.rotation = Quaternion.Euler(0, 45f, 0);
        //else if (count == 200)
        //    transform.rotation = Quaternion.Euler(0, 90f, 0);
        //else if (count == 300)
        //    transform.rotation = Quaternion.Euler(0, 135f, 0);
        //else if (count == 400)
        //    transform.rotation = Quaternion.Euler(0, 180f, 0);
        //else if (count == 500)
        //    transform.rotation = Quaternion.Euler(0, 225f, 0);
        //else if (count == 600)
        //    transform.rotation = Quaternion.Euler(0, 270f, 0);
        //else if (count == 700)
        //    Done();


    }
    public void MoveAgent(float[] act)
    {

        // 2-17~ ===============================================================================
        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;

        var shootAxis = (int)act[0];
        isFire = false;

        switch (shootAxis)
        {
            case 1:
                if (myWeaponCtrl.currentBullets > 0)
                {
                    // Debug.Log("1");
                    myWeaponCtrl.Fire();
                    isFire = true;
                }
                else
                    myWeaponCtrl.DoReload();
                break;
            //case 2:
            //    myWeaponCtrl.DoReload();
            //    break;
        }
    }

    //public override void AgentReset()
    //{
    //    count = 0;
    //    isKill = false;
    //    isShotMiss = false;
    //    myWeaponCtrl.currentBullets = 99999999;
    //    transform.rotation = Quaternion.identity;
    //    //ResetTarget();
    //}

    //void ResetTarget()
    //{

    //    //~2 28 ========================================== 16받았음ㄴ
    //    int liveCount = 0;

    //    liveCount = 0;
    //    for (int i = 0; i < targets.Length; i++)
    //    {
    //        int random = Random.Range(0, 2);
    //        //Debug.Log(random);
    //        if (random == 0)
    //            targets[i].gameObject.SetActive(false);
    //        else
    //        {
    //            liveCount++;
    //            targets[i].gameObject.SetActive(true);
    //        }
    //    }
    //}

}
