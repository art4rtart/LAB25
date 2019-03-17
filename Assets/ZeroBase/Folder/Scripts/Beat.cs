using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
	[Header("Beat movement")]
	public AnimationCurve defaultAnimCurve;
	public AnimationCurve adrenalineAnimCurve;

	[HideInInspector] public bool defaultTrigger;
	[HideInInspector] public bool adrenalineTrigger;
	float value;
	float height;

	void Start()
	{
		Invoke("DefaultMovement", 2f);
	}

	void Update()
    {
		if (Input.GetKeyDown(KeyCode.A))
		{
			defaultTrigger = true;
			adrenalineTrigger = false;
			value = 0;
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			adrenalineTrigger = true;
			defaultTrigger = false;
			value = 0;
		}

		if (adrenalineTrigger)
		{
			AdrenalineMovement();
		}

		else if (defaultTrigger)
		{
			DefaultMovement();
		}

		this.transform.position = new Vector3(this.transform.position.x, height * 4f, this.transform.position.z);
	}

	public void DefaultMovement()
	{
		defaultTrigger = true;
		value += Time.deltaTime;
		height = defaultAnimCurve.Evaluate(value);
	}

	public void AdrenalineMovement()
	{
		adrenalineTrigger = true;
		value += Time.deltaTime;
		height = adrenalineAnimCurve.Evaluate(value);
	}
}
