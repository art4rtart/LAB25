using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hover : MonoBehaviour
{
	[Header("Default")]
	Lobby lobby;
	SceneChanger sceneChanger;

	[Header("TextMeshPro")]
	public TMP_FontAsset glowFont;
	public TMP_FontAsset defaultFont;
	public TextMeshProUGUI[] textMeshProText;
	public Color defaultTextColor;
	public Color HighlightTextColor;

	[Header("Blur")]
	float blurValue;
	float blurSpeed = 15f;
	bool blur;
	bool firstTrigger = true;

	void Awake()
	{
		lobby = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Lobby>();
		sceneChanger = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneChanger>();
	}

	void Update()
	{
		if (!blur)
		{
			if (!firstTrigger) blurValue = blurValue - blurSpeed * Time.deltaTime;
			lobby.blurRenderer.GetComponent<Renderer>().sharedMaterial.SetFloat("_Size", blurValue);
			if (blurValue < 0) lobby.blurRenderer.SetActive(false);
		}

		else
		{
			lobby.blurRenderer.SetActive(true);
			blurValue = Mathf.Clamp(blurValue + blurSpeed * Time.deltaTime, 0, 3.5f);
			lobby.blurRenderer.GetComponent<Renderer>().sharedMaterial.SetFloat("_Size", blurValue);
			Debug.Log(blurValue);
		}
	}

	public void PlayCampaignClicked()
	{
		StartCoroutine(sceneChanger.LoadAsynchronously(sceneChanger.nextSceneName));
	}

	public void AchievementsClicked()
	{

	}

	public void OptionsClicked()
	{

	}

	public void NoEnter()
	{
		textMeshProText[3].font = glowFont;
	}

	public void NoExit()
	{
		textMeshProText[3].font = defaultFont;
	}

	public void YesEnter()
	{
		textMeshProText[2].font = glowFont;
	}

	public void YesExit()
	{
		textMeshProText[2].font = defaultFont;
	}

	public void QuitClicked()
	{
		blur = true;
		lobby.QuitMesseageBox.SetActive(blur);
		if(!firstTrigger) lobby.QuitMesseageBox.GetComponent<Animator>().SetTrigger("FadeIn");
	}

	public void ExitYesClicked()
	{
		Application.Quit();
	}

	public void ExitNoClicked()
	{
		lobby.QuitMesseageBox.SetActive(true);
		firstTrigger = blur = false;
		lobby.QuitMesseageBox.GetComponent<Animator>().SetTrigger("FadeOut");
	}

	public void HoverEnter()
	{
		//Hover Sound

		for (int i = 0; i < textMeshProText.Length; i++) {
			textMeshProText[i].color = HighlightTextColor;
			textMeshProText[i].font = glowFont;
		}
	}

	public void HoverExit()
	{
		for (int i = 0; i < textMeshProText.Length; i++)
		{
			textMeshProText[i].color = defaultTextColor;
			textMeshProText[i].font = defaultFont;
		}
	}
}
