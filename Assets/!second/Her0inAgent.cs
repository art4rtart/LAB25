using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class Her0inAgent : Agent
{
	private Rigidbody agentRB;

	public Transform pivotTransform;
	public Transform target;
	public float agentRunSpeed = 800f;

	private bool targetEaten = false;
	private bool dead = false;

	PathFinder pathFinder;

	public override void InitializeAgent()
	{
		pathFinder = GetComponent<PathFinder>();
		pathFinder.enabled = true;

		agentRB = GetComponent<Rigidbody>();
		base.InitializeAgent();
	}

	void ResetTarget()
	{
		targetEaten = false;

		//learning
		//Vector3 randomPos = new Vector3(Random.Range(-10f, 10f), 0.5f, Random.Range(-10f, 10f));
		//target.position = randomPos + pivotTransform.position;
	}

	public override void AgentReset()
	{
		//learning
		//Vector3 randomPos = new Vector3(Random.Range(-10f, 10f), 0.5f, Random.Range(-10f, 10f));
		//transform.position = randomPos + pivotTransform.position;

		dead = false;
		agentRB.velocity = Vector3.zero;
		ResetTarget();
	}

	public override void CollectObservations()
	{
		// position
		Vector3 distanceToTarget = target.position - transform.position;

		AddVectorObs(Mathf.Clamp(distanceToTarget.x / 10f, -1f, 1f));
		AddVectorObs(Mathf.Clamp(distanceToTarget.z / 10f, -1f, 1f));

		Vector3 relativePos = transform.position - pivotTransform.position;

		AddVectorObs(Mathf.Clamp(relativePos.x/ 10f, -1f, 1f));
		AddVectorObs(Mathf.Clamp(relativePos.z / 10f, -1f, 1f));

		AddVectorObs(Mathf.Clamp(agentRB.velocity.x /10f, -1f, 1f));
		AddVectorObs(Mathf.Clamp(agentRB.velocity.z / 10f, -1f, 1f));
	}

	public override void AgentAction(float[] act, string textAction)
	{
		Vector3 lookPosition = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
		this.transform.LookAt(lookPosition);

		AddReward(-0.001f);

		float horizontalInput = act[0];
		float verticalInput = act[1];

		agentRB.AddForce(horizontalInput * agentRunSpeed, 0f, verticalInput * agentRunSpeed);

		if (targetEaten)
		{
			AddReward(5.0f);
			ResetTarget();
		}

		else if (dead)
		{
			AddReward(-0.001f);
		}
	}

	void OnCollisionStay(Collision other)
	{
		if (other.gameObject.CompareTag("wall"))
		{
			// after learning
			// agentRB.AddForce(Vector3.back * 10f, ForceMode.VelocityChange);

			// dead = true;
		}
	}

	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.CompareTag("wall"))
		{
			// after learning
			//agentRB.velocity = Vector3.zero;
			//agentRB.angularVelocity = Vector3.zero;

			// learning
			// dead = false;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("goal"))
		{
			//agentRB.velocity = Vector3.zero;
			//agentRB.angularVelocity = Vector3.zero;
			targetEaten = true;
		}
	}
}
