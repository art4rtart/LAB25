using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoreExploder : MonoBehaviour
{
	public AudioClip[] audioClip;
	public GlassBreak[] glassBreaker;
	public bool isReadyToOverPower;
	public bool isOverPowering;

	float currentExplodeRate = 0;
	public float maxExplodeRate = 3;
	bool isExploded;

	public ParticleSystem[] sparkParticle;
	public ParticleSystem[] explodeParticle;

	ParticleSystem[] spartkPs;
	ParticleSystem[] explodePs;

	public TextMeshProUGUI explanationText;
	public TextMeshProUGUI explodeRateText;
	public CameraShake.Properties testProperties;

	void Awake() {
		spartkPs = sparkParticle;
		explodePs = explodeParticle;
		for (int i = 0; i < explodePs.Length; i++) explodePs[i].Stop();
	}

	void Update()
	{
		if (!isReadyToOverPower) return;

		if (Input.GetKey(KeyCode.F))
		{
			currentExplodeRate = Mathf.Clamp(currentExplodeRate +=Time.deltaTime, 0, maxExplodeRate);
			isOverPowering = true;
		}

		if(Input.GetKeyUp(KeyCode.F))
		{
			if(!isExploded) currentExplodeRate = 0;
			isOverPowering = false;
		}
		
		if(!isExploded && currentExplodeRate >= maxExplodeRate)
		{
			StartCoroutine(Explode());
			isExploded = true;
		}

		explodeRateText.text = (Mathf.Floor(currentExplodeRate * 100) * 0.01f).ToString();

		EffectUpdate();
	}

	IEnumerator Explode()
	{
		yield return new WaitForSeconds(2f);
		// light go down
		StartCoroutine(LightChanger());

		// explode particle play
		for (int i = 0; i < explodePs.Length; i++) explodePs[i].Play();

		// explode sound play

		// glass break
		for (int i = 0; i < glassBreaker.Length; i++) glassBreaker[i].Break();

		// cameraShake
		//FindObjectOfType<CameraShake>().StartShake(testProperties);
		//yield return new WaitForSeconds(.5f);
		//FindObjectOfType<CameraShake>().StartShake(testProperties);

		// Spark Particle Stop
		for (int i = 0; i < spartkPs.Length; i++) 	spartkPs[i].Stop();


		explodeRateText.text = "0";

		explanationText.color = new Color(explanationText.color.r, explanationText.color.g, explanationText.color.b, 0.5f);
		explodeRateText.color = new Color(explodeRateText.color.r, explodeRateText.color.g, explodeRateText.color.b, 0.5f);

		isReadyToOverPower = false;

		yield return null;
	}

	public Light pointLight;

	IEnumerator LightChanger()
	{
		float alpha = 0;

		while(alpha <= .5f)
		{
			pointLight.intensity = Random.Range(1f, 2f);
			alpha += Time.deltaTime;
			yield return null;
		}

		pointLight.intensity = 1;

		yield return null;
	}

	void EffectUpdate()
	{
		if (isOverPowering)
		{
			for (int i = 0; i < spartkPs.Length; i++)
			{
				spartkPs[i].startLifetime = Mathf.Clamp(spartkPs[i].startLifetime += 0.05f, 0, 1.5f);
			}
		}

		else
		{
			for (int i = 0; i < spartkPs.Length; i++)
			{
				spartkPs[i].startLifetime = Mathf.Clamp(spartkPs[i].startLifetime -= 0.1f, 0, 1.5f);
			}
		}
	}
}
