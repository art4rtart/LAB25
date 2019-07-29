using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Hover : MonoBehaviour
{
	[Header("Default")]
	Lobby lobby;
	LevelLoader levelLoader;
	AudioManager audioManager;

	[Header("TextMeshPro")]
	public TMP_FontAsset glowFont;
	public TMP_FontAsset defaultFont;
	public TextMeshProUGUI[] textMeshProText;
	public Color defaultTextColor;
	public Color highlightTextColor;

	[Header("Blur")]
	float blurValue;
	float blurSpeed = 15f;
	bool blur;
	bool blurTrigger = false;

	public GameObject highlightMenu;
	bool typeTrigger2;
	public TextMeshProUGUI[] campaignSubMenu;
	public TextMeshProUGUI[] achievementSubMenu;
	public TextMeshProUGUI[] optionSubMenu;

	public string[] loadingNames = { "Loading1", "Loading2", "Loading3", "Loading4", "Loading5" };
	public string[] stageNames = { "Stage1", "Stage2", "Stage3", "Stage4", "Stage5" };
	public static bool loadCredit;

	void Awake()
	{
		audioManager = FindObjectOfType<AudioManager>();
		lobby = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Lobby>();
	}

	void Start()
	{
		loadCredit = false;
	}

	void Update()
	{
		if (blurTrigger && blur)
		{
			lobby.blurRenderer.SetActive(true);
			blurValue = Mathf.Clamp(blurValue + blurSpeed * Time.deltaTime, 0, 1.5f);
			lobby.blurRenderer.GetComponent<Renderer>().sharedMaterial.SetFloat("_Size", blurValue);
		}

		else if (blurTrigger && !blur)
		{
			blurValue = Mathf.Clamp(blurValue - blurSpeed * Time.deltaTime, 0, 1.5f);
			lobby.blurRenderer.GetComponent<Renderer>().sharedMaterial.SetFloat("_Size", blurValue);
			if (blurValue <= 0) lobby.blurRenderer.SetActive(false);
		}

		if (lobby.menuAnimator.GetCurrentAnimatorStateInfo(0).IsName("MenuAnimationFadeOut") &&
			lobby.menuAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
		{
			if (loadCredit) return;

			int index = Mathf.Abs(Lobby.spriteIndex);
			LevelLoader.sceneName = stageNames[index];
			SceneManager.LoadScene(loadingNames[Random.Range(0, loadingNames.Length)]);
		}

		if (lobby.highlightMenuAnimator.isActiveAndEnabled &&
			lobby.highlightMenuAnimator.GetCurrentAnimatorStateInfo(0).IsName("OpenHighlightMenu") &&
			lobby.highlightMenuAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
		{
			if (!typeTrigger2)
			{
				lobby.isSubMenuClosed = true;
				typeTrigger2 = true;
			}
		}

		if (lobby.isSubMenuClosed)
		{
			if (lobby.menuIndex == 1)
			{
				for (int i = 0; i < campaignSubMenu.Length; i++)
				{
					if (!campaignSubMenu[i].GetComponent<TypeSentence>().isStarted)
					{
						campaignSubMenu[i].enabled = true;
						audioManager.Play("TypingSound");
						StartCoroutine(campaignSubMenu[i].GetComponent<TypeSentence>().Type(campaignSubMenu[i].GetComponent<TypeSentence>().sentence));
					}
				}
			}

			else if(lobby.menuIndex == 2)
			{
				for (int i = 0; i < achievementSubMenu.Length; i++)
				{
					if (!achievementSubMenu[i].GetComponent<TypeSentence>().isStarted)
					{
						achievementSubMenu[i].enabled = true;
						audioManager.Play("TypingSound");
						StartCoroutine(achievementSubMenu[i].GetComponent<TypeSentence>().Type(achievementSubMenu[i].GetComponent<TypeSentence>().sentence));
					}
				}
			}

			else if (lobby.menuIndex == 3)
			{
				for (int i = 0; i < optionSubMenu.Length; i++)
				{
					if (!optionSubMenu[i].GetComponent<TypeSentence>().isStarted)
					{
						optionSubMenu[i].enabled = true;
						audioManager.Play("TypingSound");
						StartCoroutine(optionSubMenu[i].GetComponent<TypeSentence>().Type(optionSubMenu[i].GetComponent<TypeSentence>().sentence));
					}
				}
			}
		}
	}

	public void PlayCampaignClicked()
	{
		if (!lobby.isSomethingClicked)
		{
			lobby.menuIndex = 1;
			audioManager.Play("DefaultClickSound");
			highlightMenu.SetActive(true);

			lobby.highlightMenuAnimator.SetBool("Highlight", true);

			for (int i = 0; i < this.textMeshProText.Length; i++)
			{
				this.textMeshProText[i].color = highlightTextColor;
				this.textMeshProText[i].font = glowFont;
			}

			lobby.isSomethingClicked = true;
			typeTrigger2 = false;
		}

		else return;
	}

	public void AchievementsClicked()
	{
		if (!lobby.isSomethingClicked)
		{
			lobby.menuIndex = 2;
			audioManager.Play("DefaultClickSound");
			highlightMenu.SetActive(true);

			lobby.highlightMenuAnimator.SetBool("Highlight", true);

			for (int i = 0; i < textMeshProText.Length; i++)
			{
				this.textMeshProText[i].color = highlightTextColor;
				this.textMeshProText[i].font = glowFont;
			}

			lobby.isSomethingClicked = true;
			typeTrigger2 = false;
		}

		else return;
	}

	public void OptionsClicked()
	{
		if (!lobby.isSomethingClicked)
		{
			lobby.menuIndex = 3;
			print(textMeshProText.Length);

			audioManager.Play("DefaultClickSound");
			highlightMenu.SetActive(true);

			lobby.highlightMenuAnimator.SetBool("Highlight", true);

			for (int i = 0; i < textMeshProText.Length; i++)
			{
				this.textMeshProText[i].color = highlightTextColor;
				this.textMeshProText[i].font = glowFont;
			}

			lobby.isSomethingClicked = true;
			typeTrigger2 = false;
		}

		else return;
	}

	public void NoEnter()
	{
		audioManager.Play ("HoverSound");
		textMeshProText[3].font = glowFont;
	}

	public void NoExit()
	{
		textMeshProText[3].font = defaultFont;
	}

	public void YesEnter()
	{
		audioManager.Play("HoverSound");
		textMeshProText[2].font = glowFont;
	}

	public void YesExit()
	{
		textMeshProText[2].font = defaultFont;
	}

	public void QuitClicked()
	{
		audioManager.Play("DefaultClickSound");
		blur = true;
		blurTrigger = true;
		lobby.QuitMesseageBox.SetActive(true);
		textMeshProText[2].font = defaultFont;
		textMeshProText[3].font = defaultFont;
		lobby.QuitMesseageBox.GetComponent<Animator>().SetTrigger("FadeIn");
		lobby.isSomethingClicked = true;
	}

	public void ExitYesClicked()
	{
		audioManager.Play("DefaultClickSound");
		blur = false;
		Application.Quit();
	}

	public void ExitNoClicked()
	{
		audioManager.Play("DefaultClickSound");
		blur = false;
		lobby.QuitMesseageBox.GetComponent<Animator>().SetTrigger("FadeOut");
		for (int i = 0; i < textMeshProText.Length; i++)
		{
			textMeshProText[i].color = defaultTextColor;
			textMeshProText[i].font = defaultFont;
		}
		highlightMenu.SetActive(false);
		lobby.isSomethingClicked = false;
	}

	public void HoverEnter()
	{
		if (!lobby.isSomethingClicked && lobby.menuAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
		{
			audioManager.Play("HoverSound");
			highlightMenu.SetActive(true);
			for (int i = 0; i < textMeshProText.Length; i++)
			{
				textMeshProText[i].color = highlightTextColor;
				textMeshProText[i].font = glowFont;
			}
		}

		else return;
	}

	public void HoverExit()
	{
		if (!lobby.isSomethingClicked)
		{
			highlightMenu.SetActive(false);

			for (int i = 0; i < textMeshProText.Length; i++)
			{
				textMeshProText[i].color = defaultTextColor;
				textMeshProText[i].font = defaultFont;
			}
		}

		else return;
	}

	public void NewGameHoverEnter()
	{
		audioManager.Play("HoverSound");
		campaignSubMenu[0].color = highlightTextColor;
		campaignSubMenu[0].font = glowFont;
	}

	public void NewGameHoverExit()
	{
		campaignSubMenu[0].color = defaultTextColor;
		campaignSubMenu[0].font = defaultFont;
	}

	public void ContinueGameHoverEnter()
	{
		audioManager.Play("HoverSound");
		campaignSubMenu[1].color = highlightTextColor;
		campaignSubMenu[1].font = glowFont;
	}

	public void ContinueGameHoverExit()
	{
		campaignSubMenu[1].color = defaultTextColor;
		campaignSubMenu[1].font = defaultFont;
	}

	public TextMeshProUGUI backText;
	public void BackHoverEnter()
	{
		audioManager.Play("HoverSound");
		backText.color = highlightTextColor;
		backText.font = glowFont;
	}

	public void SettingsHoverEnter()
	{
		audioManager.Play("HoverSound");
		optionSubMenu[0].color = highlightTextColor;
		optionSubMenu[0].font = glowFont;
	}

	public void SettingsHoverExit()
	{
		optionSubMenu[0].color = defaultTextColor;
		optionSubMenu[0].font = defaultFont;
	}

	public void CreditHoverEnter()
	{
		audioManager.Play("HoverSound");
		optionSubMenu[1].color = highlightTextColor;
		optionSubMenu[1].font = glowFont;
	}

	public void CreditHoverExit()
	{
		optionSubMenu[1].color = defaultTextColor;
		optionSubMenu[1].font = defaultFont;
	}

	public void BackHoverExit()
	{
		backText.color = defaultTextColor;
		backText.font = defaultFont;
	}

	public void BackHoverClicked()
	{
		audioManager.Play("DefaultClickSound");
		lobby.highlightMenuAnimator.SetBool("Highlight", false);

		if (lobby.menuIndex == 1)
		{
			for (int i = 0; i < campaignSubMenu.Length; i++)
			{
				campaignSubMenu[i].text = "";
				campaignSubMenu[i].GetComponent<TypeSentence>().isStarted = false;
				campaignSubMenu[i].enabled = false;
			}
		}

		else if (lobby.menuIndex == 2)
		{
			for (int i = 0; i < achievementSubMenu.Length; i++)
			{
				achievementSubMenu[i].text = "";
				achievementSubMenu[i].GetComponent<TypeSentence>().isStarted = false;
				achievementSubMenu[i].enabled = false;
			}
		}

		else if (lobby.menuIndex == 3)
		{
			for (int i = 0; i < optionSubMenu.Length; i++)
			{
				optionSubMenu[i].text = "";
				optionSubMenu[i].GetComponent<TypeSentence>().isStarted = false;
				optionSubMenu[i].enabled = false;
			}
		}

		for (int i = 0; i < textMeshProText.Length; i++)
		{
			textMeshProText[i].color = defaultTextColor;
			textMeshProText[i].font = defaultFont;
		}

		highlightMenu.SetActive(false);
		lobby.isSubMenuClosed = false;
		lobby.menuIndex = 0;
		lobby.isSomethingClicked = false;
	}

	public AudioSource bgm;

	public void NewGameLoad()
	{
		audioManager.Play("DefaultClickSound");
		lobby.menuAnimator.SetTrigger("FadeOut");
		lobby.highlightMenuAnimator.SetTrigger("Fade");

		StartCoroutine(FadeVolume());
	}

	public void ContinueGameLoad()
	{
		audioManager.Play("DefaultClickSound");
		lobby.menuAnimator.SetTrigger("FadeOut");
		lobby.highlightMenuAnimator.SetTrigger("Fade");

		StartCoroutine(FadeVolume());
	}

	public void CreditLoad()
	{
		loadCredit = true;
		SceneMaster.SaveCurrentSceneName();

		audioManager.Play("DefaultClickSound");
		lobby.menuAnimator.SetTrigger("FadeOut");
		lobby.highlightMenuAnimator.SetTrigger("Fade");

		StartCoroutine(FadeVolume());
	}

	IEnumerator FadeVolume()
	{
		while (bgm.volume >= 0f)
		{
			bgm.volume -= Time.deltaTime;

			if (bgm.volume <= 0.3f && loadCredit) SceneManager.LoadScene("Credit");
			yield return null;
		}
	}
}
