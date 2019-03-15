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
	UniverseController universeController;

	void Awake()
	{
		parentAnimator = GetComponent<Animator>();
		universeController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UniverseController>();
	}

	void Update()
	{
		if (!setChildAnim && universeController.trigger)
		{
			childAnimator = this.transform.GetChild(1).GetComponent<Animator>();
			setChildAnim = true;
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
}
