using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Title : MonoBehaviour
{
	AudioManager audioManager;
	public Animator cameraAnimator;
	public Animator titleAnimator;
	public TextMeshProUGUI pressAnyKey;
	public float glowSpeed;
	float glowDir = 1f;
	float glowValue;
	float minValue = 0f;

	void OnEnable()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.None;
	}

	void Awake()
	{
		audioManager = FindObjectOfType<AudioManager>();
		Cursor.visible = false;
		pressAnyKey.fontSharedMaterial.SetFloat("_GlowPower", .0f);
	}

	void Update()
	{
		PressAnyKeyTrigger();
	}

	void PressAnyKeyTrigger()
	{
		if (pressAnyKey.color.a >= 0.8f)
		{
			glowValue = Mathf.Clamp(glowValue += glowSpeed * glowDir * Time.deltaTime, minValue, 0.5f);

			if (glowValue == 0.5f)
			{
				glowDir *= -1f;
				minValue = 0.2f;
			}

			else if (glowValue == 0.2f)
			{
				glowDir *= -1f;
			}

			Cursor.visible = true;
			pressAnyKey.fontSharedMaterial.SetFloat("_GlowPower", glowValue);
		}

		if (titleAnimator.GetCurrentAnimatorStateInfo(0).IsName("TitleAnimationFadeIn") &&
			titleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
		{
			if (Input.GetMouseButtonDown(0))
			{
				audioManager.Play("SpecialClickSound");
				cameraAnimator.SetTrigger("Pressed");
				titleAnimator.SetTrigger("Pressed");
			}
		}

		else return;
	}
}
