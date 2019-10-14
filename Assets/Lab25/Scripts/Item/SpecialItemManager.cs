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
	public Material lightMaterial;

	bool heraterActivateTrigger;
	float alpha;

	void Start()
    {
		alpha = 0.45f;
		lightMaterial.color = new Color(lightMaterial.color.r, lightMaterial.color.g, lightMaterial.color.b, 0);
	}

	void Update()
	{
		AnimatorStateInfo animStateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);

		if (animStateInfo.IsName("Hearter") && animStateInfo.normalizedTime > 0f && !heraterActivateTrigger)
		{
			StopAllCoroutines();

			//playerAnim.ResetTrigger("endHearter");
			//playerAnim.ResetTrigger("endBomb");

			StartCoroutine(LightOn());
			heaterScreen.SetActive(true);
			heraterAnim.SetBool("activate", true);
			StartCoroutine(Glitch());
			heraterActivateTrigger = true;
		}

		else if (animStateInfo.IsName("harter_d") && animStateInfo.normalizedTime > 0.1f && heraterActivateTrigger)
		{
			StopAllCoroutines();
			StartCoroutine(LightOff());
			heraterAnim.SetBool("activate", false);
			heraterActivateTrigger = false;
		}

		else if (animStateInfo.IsName("EndToDo(AK)") && animStateInfo.normalizedTime >= 1f)
		{
			StopAllCoroutines();
			heaterScreen.SetActive(false);
		}
	}

	IEnumerator LightOn()
	{
		float currentAlphaValue = 0;

		while (currentAlphaValue <= alpha)
		{
			currentAlphaValue += 0.5f * Time.deltaTime;
			lightMaterial.color = new Color(lightMaterial.color.r, lightMaterial.color.g, lightMaterial.color.b, currentAlphaValue);
			yield return null;
		}
	}

	IEnumerator LightOff()
	{
		float currentAlphaValue = alpha;

		while (currentAlphaValue >= 0)
		{
			currentAlphaValue -= Time.deltaTime;
			lightMaterial.color = new Color(lightMaterial.color.r, lightMaterial.color.g, lightMaterial.color.b, currentAlphaValue);
			yield return null;
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
