using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTrigger : MonoBehaviour
{
	public Animator fadeAnimator;

	public GameObject noWeaponModel;
	public GameObject swapModel;
	public bool checkOnce;

	float eTime;

	void Update()
	{
		Debug.Log(checkOnce);

		if (checkOnce)
		{
			Debug.Log("HI");
			eTime += Time.deltaTime;

			if (eTime >= 1f)
			{
				noWeaponModel.SetActive(false);
				swapModel.SetActive(true);
				this.gameObject.SetActive(false);
			}
		}
	}

	public void fadeInOut()
	{
		fadeAnimator.SetTrigger("FadeIn");
		checkOnce = true;
	}
}
