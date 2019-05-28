using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class FindBlockAgent : Agent
{
	Her0inAgent her0inAgent;

	public GameObject ground;
	public GameObject area;

	[HideInInspector]
	public Bounds areaBounds;
	FindBlockAcademy academy;

	public GameObject block;

	[HideInInspector]
	public FindGoalDetect goalDetect;

	public bool useVectorObs;

	Rigidbody blockRB;
	Rigidbody agentRB; 
	Material groundMaterial;
	RayPerception rayPer;

	Renderer groundRenderer;

	public GameObject[] triggers;

	void Awake()
	{
		her0inAgent = GetComponent<Her0inAgent>();
		academy = FindObjectOfType<FindBlockAcademy>();
	}

	public override void InitializeAgent()
	{
		base.InitializeAgent();
		goalDetect = block.GetComponent<FindGoalDetect>();
		goalDetect.agent = this;
		rayPer = GetComponent<RayPerception>();

		agentRB = GetComponent<Rigidbody>();
		blockRB = block.GetComponent<Rigidbody>();

		//areaBounds = ground.GetComponent<Collider>().bounds;
		//groundRenderer = ground.GetComponent<Renderer>();
		//groundMaterial = groundRenderer.material;

		this.transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
	}

	public override void CollectObservations()
	{
		if (useVectorObs)
		{
			var rayDistance = 15f;
			float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f };
			var detectableObjects = new[] { "block", "goal", "wall" };
			AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
			AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1.5f, 0f));
		}
	}

	// Learning
	//public Vector3 GetRandomSpawnPos()
	//{
	//	bool foundNewSpawnLocation = false;
	//	Vector3 randomSpawnPos = Vector3.zero;
	//	while (foundNewSpawnLocation == false)
	//	{
	//		float randomPosX = Random.Range(-areaBounds.extents.x * academy.spawnAreaMarginMultiplier,
	//							areaBounds.extents.x * academy.spawnAreaMarginMultiplier);

	//		float randomPosZ = Random.Range(-areaBounds.extents.z * academy.spawnAreaMarginMultiplier,
	//										areaBounds.extents.z * academy.spawnAreaMarginMultiplier);
	//		randomSpawnPos = ground.transform.position + new Vector3(randomPosX, 1f, randomPosZ);
	//		if (Physics.CheckBox(randomSpawnPos, new Vector3(2.5f, 0.01f, 2.5f)) == false)
	//		{
	//			foundNewSpawnLocation = true;
	//		}
	//	}
	//	return randomSpawnPos;
	//}

	public void IScoredAGoal()
	{
		AddReward(5f);

		her0inAgent.enabled = true;

		for(int i =0; i < triggers.Length; i++)
		{
			triggers[i].SetActive(false);
		}

		this.enabled = false;

		// Learning
		//Done();
		//StartCoroutine(GoalScoredSwapGroundMaterial(academy.goalScoredMaterial, 0.5f));
	}

	IEnumerator GoalScoredSwapGroundMaterial(Material mat, float time)
	{
		groundRenderer.material = mat;
		yield return new WaitForSeconds(time); // Wait for 2 sec
		groundRenderer.material = groundMaterial;
	}

	public void MoveAgent(float[] act)
	{

		Vector3 dirToGo = Vector3.zero;
		Vector3 rotateDir = Vector3.zero;

		int action = Mathf.FloorToInt(act[0]);

		switch (action)
		{
			case 1:
				dirToGo = transform.forward * 1f;
				break;
			case 2:
				dirToGo = transform.forward * -1f;
				break;
			case 3:
				rotateDir = transform.up * 1f;
				break;
			case 4:
				rotateDir = transform.up * -1f;
				break;
			case 5:
				dirToGo = transform.right * -0.75f;
				break;
			case 6:
				dirToGo = transform.right * 0.75f;
				break;
		}
		transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
		agentRB.AddForce(dirToGo * academy.agentRunSpeed,
						 ForceMode.VelocityChange);
	}

	public override void AgentAction(float[] vectorAction, string textAction)
	{
		MoveAgent(vectorAction);
		AddReward(-1f / agentParameters.maxStep);
	}

	//void ResetBlock()
	//{
	//	block.transform.position = GetRandomSpawnPos();

	//	blockRB.velocity = Vector3.zero;

	//	blockRB.angularVelocity = Vector3.zero;
	//}

	//public override void AgentReset()
	//{
	//	int rotation = Random.Range(0, 4);
	//	float rotationAngle = rotation * 90f;
	//	area.transform.Rotate(new Vector3(0f, rotationAngle, 0f));

	//	ResetBlock();
	//	transform.position = GetRandomSpawnPos();
	//	agentRB.velocity = Vector3.zero;
	//	agentRB.angularVelocity = Vector3.zero;
	//}
}
