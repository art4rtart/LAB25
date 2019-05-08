using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItemManager : MonoBehaviour
{
	public Animator playerAnim;
	public Animator heraterAnim;
	public GameObject heaterScreen;
	public AnalogGlitch glitch;
	public bool hasHearter;

	bool heraterActivateTrigger;
	void Start()
    {

	}

	void Update()
	{
		AnimatorStateInfo animStateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);

		if (animStateInfo.IsName("useHarter") && animStateInfo.normalizedTime > 0.1f && !heraterActivateTrigger)
		{
			heaterScreen.SetActive(true);
			heraterAnim.SetBool("activate", true);
			StartCoroutine(Glitch());
			heraterActivateTrigger = true;
		}

		else if (animStateInfo.IsName("endHearter") && animStateInfo.normalizedTime > 0.1f && heraterActivateTrigger)
		{
			heraterAnim.SetBool("activate", false);
			heraterActivateTrigger = false;
		}

		else if (animStateInfo.IsName("endHearter") && animStateInfo.normalizedTime >= 1f)
		{
			StopAllCoroutines();
			heaterScreen.SetActive(false);
		}
	}

	IEnumerator Glitch()
	{
		float lerpSpeed = 0;
		float speed = 0.35f;

		while(glitch.scanLineJitter + glitch.horizontalShake + glitch.colorDrift >= 0)
		{
			glitch.scanLineJitter = Mathf.Lerp(glitch.scanLineJitter, 0, lerpSpeed);
			glitch.horizontalShake = Mathf.Lerp(glitch.horizontalShake, 0, lerpSpeed);
			glitch.colorDrift = Mathf.Lerp(glitch.colorDrift, 0, lerpSpeed);

			lerpSpeed += speed * Time.deltaTime;

			yield return null;
		}
	}
}
