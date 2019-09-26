using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PostProcessing;

public class UTRSManager : MonoBehaviour
{
	public enum CurrentMenu { None, Main, Item, Upgrade, Barricade};
	public CurrentMenu MenuState;

	public Camera fpsCamera;
	public GameObject mainCamera;
	public GameObject gameManager;

	public static UTRSManager Instance
	{
		get
		{
			if (instance != null)
				return instance;
			instance = FindObjectOfType<UTRSManager>();
			return instance;
		}
	}
	private static UTRSManager instance;

	public TextMeshProUGUI goldText;

	[HideInInspector] public float addGold;
	public float totalGold;

	public GameObject mainMenu;
	public GameObject shopMenu;
	public GameObject upgradeMenu;
	public GameObject barricadeAddMenu;

	public PostProcessingProfile ppProfile;

	[HideInInspector] public Transform player;
	WeaponCtrl weaponCtrl;
	WeaponSway weaponSway;

	void UpdateText()
	{
		goldText.text = totalGold.ToString();
	}

	void Start()
	{
		UpdateText();
		SetPostprocessing(1f);
		player = FindObjectOfType<PlayerCtrl>().transform;
		weaponCtrl = FindObjectOfType<WeaponCtrl>();
		weaponSway = FindObjectOfType<WeaponSway>();
	}

	void OnApplicationQuit()
	{
		SetPostprocessing(0.09f);
	}

	void SetPostprocessing(float bloomValue)
	{
		BloomModel.Settings bloomSettings = ppProfile.bloom.settings;
		bloomSettings.bloom.intensity = bloomValue;
		ppProfile.bloom.settings = bloomSettings;
	}

	public bool showMenuOpen = false;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Tab) && (MenuState == CurrentMenu.Main || MenuState == CurrentMenu.None))
		{
			mainCamera.transform.position = fpsCamera.transform.position;
			mainCamera.transform.eulerAngles = fpsCamera.transform.eulerAngles;
			mainCamera.SetActive(!showMenuOpen);
			gameManager.SetActive(showMenuOpen);
			weaponCtrl.enabled = !weaponCtrl.enabled;
			weaponSway.enabled = !weaponSway.enabled;

			Cursor.visible = !showMenuOpen;
			Cursor.lockState = CursorLockMode.None;

			fpsCamera.enabled = !fpsCamera.enabled;
			player.GetComponent<PlayerCtrl>().enabled = !player.GetComponent<PlayerCtrl>().enabled;

			showMenuOpen = !showMenuOpen;

			if (showMenuOpen) MenuState = CurrentMenu.Main;
			else MenuState = CurrentMenu.None;

			StopAllCoroutines();
			StartCoroutine(Blur(showMenuOpen));
		}
	}

	public float smothness = 2f;

	public IEnumerator Blur(bool dir)
	{
		float value = 0;

		if (dir) smothness = Mathf.Abs(smothness);
		else smothness = -Mathf.Abs(smothness);

		mainMenu.GetComponent<Animator>().SetBool("FadeIn", dir);

		if (dir)
		{
			while (BoxBlur.Instance.Iterations < 30f)
			{
				BoxBlur.Instance.Iterations = Mathf.Lerp(BoxBlur.Instance.Iterations, 30f, value);
				value += Time.deltaTime * smothness;
				yield return null;
			}

		}

		else
		{
			while (BoxBlur.Instance.Iterations > 0f)
			{
				BoxBlur.Instance.Iterations = Mathf.Lerp(BoxBlur.Instance.Iterations, 0f, value);
				value -= Time.deltaTime * smothness;
				yield return null;
			}
		}
	}
}
