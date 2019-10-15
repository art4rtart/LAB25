using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTrigger : MonoBehaviour
{
	public Animator fadeAnimator;

	public GameObject noWeaponModel;
	public GameObject shotgunModel;
	public GameObject playerCamera;
	public GameObject quaterViewCamera;
	public GameObject gameManger;
	public GameObject HUD;
	public GameObject pointer;

	public bool checkOnce;

	float eTime;

	void Update()
	{
		//Debug.Log(checkOnce);

		if (checkOnce)
		{
			//Debug.Log("HI");
			eTime += Time.deltaTime;

			if (eTime >= 1f)
			{
				noWeaponModel.SetActive(false);
				shotgunModel.SetActive(true);

				quaterViewCamera.SetActive(false);
				playerCamera.SetActive(true);


				gameManger.SetActive(true);
				pointer.SetActive(true);
				HUD.SetActive(true);
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
