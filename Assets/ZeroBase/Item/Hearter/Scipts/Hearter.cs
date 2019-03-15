using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class Hearter : MonoBehaviour
{
	[Header("Default")]
	[HideInInspector]
	public bool resetTrigger;

	public PostProcessingProfile ppProfile;
	public XRayVisionController visionController;
	public Material displacementMaterial;

	float evaluateValue;
	float displacementValue;

	[Header("Effect Parameters")]
	[SerializeField]
	private Camera[] cameras = null;
	[SerializeField]
	private Vingette vingette = null;
	[SerializeField]
	private AnimationCurve _innerVingette = null;
	[SerializeField]
	private AnimationCurve _outerVingette = null;
	[SerializeField]
	private AnimationCurve _saturation = null;
	[SerializeField]
	private AnimationCurve _fov = null;
	[SerializeField]
	private AnimationCurve _timeScale = null;

	[Header("Audio Source")]
	public AudioSource[] soundEffects;

	void Start()
	{
		cameras[2].gameObject.SetActive(true);
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			soundEffects[0].PlayOneShot(soundEffects[0].clip);
			SetFrameBlending(0.4f);
			visionController.changeToVisionMaterial();
			resetTrigger = false;
		}

		if (Input.GetMouseButton(0))
		{
			evaluateValue = Mathf.Clamp(evaluateValue + Time.unscaledDeltaTime * 2.0f, 0, 1f);
			displacementValue = Mathf.Clamp(displacementValue + Time.deltaTime, 0, 0.003f);
		}

		if (Input.GetMouseButtonUp(0))
		{
			SetFrameBlending(0.0f);
			soundEffects[1].PlayOneShot(soundEffects[1].clip);
			visionController.changeToDefaultMaterial();
			resetTrigger = true;
		}

		for (int i = 0; i < cameras.Length; i++)
		{
			cameras[i].fieldOfView = _fov.Evaluate(evaluateValue);
		}

		vingette.MinRadius = _innerVingette.Evaluate(evaluateValue);
		vingette.MaxRadius = _outerVingette.Evaluate(evaluateValue);
		vingette.Saturation = _saturation.Evaluate(evaluateValue);
		displacementMaterial.SetFloat("_Magnitude", displacementValue);
		//Time.timeScale = _timeScale.Evaluate(value);

		if (resetTrigger)
		{
			resetValue();
		}
	}

	void resetValue()
	{
		if(evaluateValue > 0)
			evaluateValue = Mathf.Clamp(evaluateValue - Time.unscaledDeltaTime * 4.0f, 0, 1f);
		displacementValue = Mathf.Clamp(displacementValue - Time.deltaTime, 0, 0.003f);
	}

	void SetFrameBlending(float value)
	{
		MotionBlurModel.Settings motionblurSettings = ppProfile.motionBlur.settings;
		motionblurSettings.frameBlending = value;
		ppProfile.motionBlur.settings = motionblurSettings;
	}
}
