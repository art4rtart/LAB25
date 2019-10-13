using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BombInstallInstructor : MonoBehaviour
{
	public static BombInstallInstructor Instance
	{
		get
		{
			if (instance != null)
				return instance;
			instance = FindObjectOfType<BombInstallInstructor>();
			return instance;
		}
	}
	private static BombInstallInstructor instance;

	[HideInInspector] public TextMeshProUGUI textMeshText;

	void Awake()
    {
		textMeshText = GetComponent<TextMeshProUGUI>();
	}

	void OnEnable()
	{
		StartCoroutine(FadeInOut());	
	}

	public float fadeSpeed;
	public float savedAlpha;
	public bool isOutOfBound;
	IEnumerator FadeInOut()
	{
		float alpha = 0;
		float dir = 1f;

		HoldBombInstruction();

		while (!BombGage.Instance.canInstall) yield return null;

		while (true)
		{
			alpha = Mathf.Clamp(alpha += Time.deltaTime * fadeSpeed * dir, 0f, 1f);
			textMeshText.color = new Color(textMeshText.color.r, textMeshText.color.g, textMeshText.color.b, alpha);
			savedAlpha = alpha;
			if (alpha == 1 || alpha == 0) dir *= -1f;

			if (Input.GetKeyDown(KeyCode.B) || isOutOfBound || WeaponCtrl.Instance.isHoldingBomb)
				break;

			yield return null;
		}

		if (isOutOfBound)
		{
			StartCoroutine(FadeOut(savedAlpha, 0f));
		}

		else
		{
			textMeshText.color = new Color(textMeshText.color.r, textMeshText.color.g, textMeshText.color.b, 1f);
			InstallBombInstruction();

			while (BombGage.Instance.correctCount < 3) {
				if(isOutOfBound) { StartCoroutine(FadeOut(1f, 0f)); c = true;  break; }
				yield return null;
			}

			if(!c) StartCoroutine(FadeOut(1f, .5f));
		}
	}
	bool c = false;
	public IEnumerator FadeOut(float saveAlpha, float waitingTime)
	{
		yield return new WaitForSeconds(waitingTime);
		float alpha = saveAlpha;
		while (alpha > 0)
		{
			alpha = Mathf.Clamp(alpha -= Time.deltaTime * fadeSpeed, 0f, 1f);
			textMeshText.color = new Color(textMeshText.color.r, textMeshText.color.g, textMeshText.color.b, alpha);
			yield return null;
		}
		c = false;
		StopAllCoroutines();
		this.enabled = false;
	}

	public void HoldBombInstruction()
	{
		textMeshText.text = "PRESS B KEY TO HOLD BOMB";
	}

	public void InstallBombInstruction()
	{
		textMeshText.text = "PRESS MOUSE LEFT BUTTON TO INSTALL";
	}

	public void CorrectBombInstruction()
	{
		textMeshText.text = "PRESS MOUSE LEFT BUTTON TO CORRECT INSTALLATION";
	}
}
