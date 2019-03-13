using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Limpid : MonoBehaviour
{
	[Header("For TeamMate Script")]
	public Material mat1;
	public Material mat2;
	public Renderer limpidRenderer;

	[Header("For Limpid Script")]
	Animator parentAnimator;
	[HideInInspector]
	public Animator childAnimator;
	bool setChildAnim = false;

	void Awake()
	{
		parentAnimator = GetComponent<Animator>();
	}

	void Update()
	{
		if (!setChildAnim)
		{
			childAnimator = this.transform.GetChild(1).GetComponent<Animator>();
			setChildAnim = true;
		}

		if(Input.GetKeyDown(KeyCode.R))
		{
			parentAnimator.SetTrigger("isRun");
			childAnimator.SetTrigger("isRun");
		}
	}

	public void ChangeToNormalMaterial()
	{
		limpidRenderer.material = mat2;
	}

	public void ChangeToVisionMaterial()
	{
		limpidRenderer.material = mat1;
	}

	public void ApplyDamage(int damage)
	{

	}
}
