using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.PostProcessing;

public class Lobby : MonoBehaviour
{
	public Animator logoAnimator;
	public Animator titleAnimator;
	public Animator menuAnimator;

	public PostProcessingProfile ppProfile;
	public GameObject logoCanvas;
	public GameObject titleCanvas;
	public GameObject menuCanvas;
	bool glowTrigger = false;

	[Header("UI")]
	public Button[] menu;
	public Button leftButton;
	public Button rightButton;

	public Texture[] stageSprites;
	public RawImage[] stageRawImage;

	public TextMeshProUGUI[] stageNameTextMeshPro;
	public string[] stagename;

	[Header("Index")]
	int[] index = new int[5];
	int spriteIndex;
	bool glowText = true;

	public GameObject blurRenderer;
	public GameObject QuitMesseageBox;

	void Start()
	{
		SetPostprocessing();
		UpdateMenu();
		blurRenderer.GetComponent<Renderer>().sharedMaterial.SetFloat("_Size", 0);
	}

	void Update()
	{
		if (logoAnimator.gameObject.activeSelf &&
			logoAnimator.GetCurrentAnimatorStateInfo(0).IsName("LogoAnimation") &&
			logoAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
		{
			logoCanvas.SetActive(false);
			titleCanvas.SetActive(true);
		}

		if (titleAnimator.gameObject.activeSelf && 
			titleAnimator.GetCurrentAnimatorStateInfo(0).IsName("IntroAnimation") &&
			titleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
		{
			if(Input.GetMouseButtonDown(0))
			{
				titleAnimator.SetTrigger("Press");
				glowText = false;
				SetPostprocessing();
			}

			if (glowTrigger)
				return;

			else
			{
				StartCoroutine(GlowText());
				glowTrigger = true;
			}
		}

		if (titleAnimator.gameObject.activeSelf && 
			titleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pressed") &&
			titleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
		{
			if (!glowTrigger)
				return;

			else
			{
				titleCanvas.SetActive(false);
				menuCanvas.SetActive(true);
				menuCanvas.GetComponent<Animator>().SetTrigger("ShowMenu");
				glowTrigger = false;
			}
		}
	}

	IEnumerator GlowText()
	{
		BloomModel.Settings bloomSettings = ppProfile.bloom.settings;
		float dir = 1;
		float speed = 10f;

		while (glowText)
		{
			float value = speed * dir * Time.deltaTime;
			float calculatedValue = Mathf.Floor(value* 100) * 0.01f;

			bloomSettings.bloom.intensity += calculatedValue;

			if (bloomSettings.bloom.intensity > 15f)
			{
				bloomSettings.bloom.intensity = 15f;
				dir *= -1f;
			}

			if (bloomSettings.bloom.intensity < 2f) {
				bloomSettings.bloom.intensity = 2f;
				dir *= -1f;
			}

			ppProfile.bloom.settings = bloomSettings;

			yield return null;
		}
	}

	public void moveLeftEvent()
	{
		spriteIndex = (spriteIndex + 1) % stageSprites.Length;
		UpdateMenu();
	}

	public void moveRightButton()
	{
		spriteIndex = (spriteIndex - 1) % stageSprites.Length;
		UpdateMenu();
	}

	void UpdateMenu()
	{
		for (int i = 0; i < stageRawImage.Length; i++)
		{
			index[i] = Mathf.Abs(spriteIndex) + i;
			stageRawImage[i].texture = stageSprites[index[i] % 5];
			stageNameTextMeshPro[i].text = stagename[index[i] % 5];
		}
	}

	void SetPostprocessing()
	{
		BloomModel.Settings bloomSettings = ppProfile.bloom.settings;
		bloomSettings.bloom.intensity = 2f;
		ppProfile.bloom.settings = bloomSettings;
	}
}
