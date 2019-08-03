using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class MotionBlur : MonoBehaviour
{
	public PostProcessingProfile ppProfile;

	IEnumerator motionBlur;

	IEnumerator StartMotionBlur(float time)
	{
		float blurValue = time;

		while(blurValue >= 0)
		{
			blurValue -= Time.deltaTime;
			SetFrameBlending(blurValue);
			yield return null;
		}

		blurValue = 0;
		SetFrameBlending(blurValue);
		if (ppProfile.motionBlur.enabled) ppProfile.motionBlur.enabled = false;
		StopCoroutine(motionBlur);
	}

	void SetFrameBlending(float value)
	{
		if (!ppProfile.motionBlur.enabled) ppProfile.motionBlur.enabled = true;
		MotionBlurModel.Settings motionblurSettings = ppProfile.motionBlur.settings;
		motionblurSettings.frameBlending = value;
		ppProfile.motionBlur.settings = motionblurSettings;
	}

	public void DoMotionBlur(float time)
	{
		motionBlur = StartMotionBlur(time);
		StartCoroutine(motionBlur);
	}
}
