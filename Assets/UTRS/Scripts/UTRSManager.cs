using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PostProcessing;

public class UTRSManager : MonoBehaviour
{
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

	public enum CurrentMenu { None, Main, Item, Upgrade, Barricade};
	public CurrentMenu MenuState;

	[Header("Camera")]
	public Camera fpsCamera;
	public GameObject mainCamera;

	[Header("Text")]
	public TextMeshProUGUI goldText;
	public TextMeshProUGUI leftTimeText;

	[HideInInspector] public float addGold;
	public float totalGold;

	[Header("Menu Objects")]
	public GameObject mainMenu;
	public GameObject shopMenu;
	public GameObject upgradeMenu;
	public GameObject barricadeAddMenu;

	[Header("Time")]
	public Animator UTRSTimeAnim;
	public float leftTime;
	public float waitingTime;
	public bool startCounting;
	public float waveTotalTime;
	public float waveLeftTime;

	public GameObject gameManager;
	[HideInInspector] public Transform player;
	[HideInInspector] public WeaponCtrl weaponCtrl;
	[HideInInspector] public WeaponSway weaponSway;

	public PostProcessingProfile ppProfile;

	[Header("Sound")]
	public AudioSource[] audioSource;
	public AudioClip[] UIAudioClips;

	void Awake()
	{
		UpdateText();
		SetPostprocessing(1f);
		player = FindObjectOfType<PlayerCtrl>().transform;
		weaponCtrl = FindObjectOfType<WeaponCtrl>();
		weaponSway = FindObjectOfType<WeaponSway>();
	}

	public bool showMenuOpen = false;

	bool weaponSwap = false;

	void Start()
	{
		Animator tempAnim = player.GetComponent<Animator>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab) && (MenuState == CurrentMenu.Main || MenuState == CurrentMenu.None))
		{
			mainCamera.transform.position = fpsCamera.transform.position;
			mainCamera.transform.eulerAngles = fpsCamera.transform.eulerAngles;

			PlaySound(0);

			mainCamera.SetActive(true);
			gameManager.SetActive(false);

			weaponCtrl.enabled = false;
			weaponSway.enabled = false;

			Cursor.visible = !showMenuOpen;
			Cursor.lockState = CursorLockMode.None;

			fpsCamera.enabled = !fpsCamera.enabled;
			player.GetComponent<PlayerCtrl>().enabled = false;

			showMenuOpen = !showMenuOpen;

			if (showMenuOpen) MenuState = CurrentMenu.Main;
			else MenuState = CurrentMenu.None;

			StopAllCoroutines();
			StartCoroutine(Blur(showMenuOpen));
		}

		UpdateText();

		// weapon key settings
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if (WeaponCtrl.Instance.myWeapnType == WeaponCtrl.WEAPON.AKM || WeaponCtrl.Instance.myWeapnType == WeaponCtrl.WEAPON.SCI_FI)
			{
				weaponSwap = !weaponSwap;
				isUsingAk = !isUsingAk;
				if (!weaponSwap) { player.GetComponent<Animator>().SetTrigger("SciFitoAk"); player.GetComponent<Animator>().SetTrigger("NotUsingSciFi"); }

				player.GetComponent<Animator>().SetTrigger("doWeaponChange");
				player.GetComponent<Animator>().SetBool("ChangeToScifiGun", weaponSwap);
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			if (isUsingAk) player.GetComponent<Animator>().SetTrigger("ElectricStickToAk");
			else { player.GetComponent<Animator>().SetTrigger("ElectricStickToSciFi"); player.GetComponent<Animator>().SetBool("ChangeToScifiGun", true); }

			if (WeaponCtrl.Instance.myWeapnType == WeaponCtrl.WEAPON.SCI_FI)
			{
				player.GetComponent<Animator>().SetBool("ChangeToScifiGun", false);
				player.GetComponent<Animator>().SetTrigger("SciFitoElectricStick");
				player.GetComponent<Animator>().SetTrigger("NotUsingSciFi");
			}
		}
	}

	bool isUsingAk = true;

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

	void UpdateText()
	{
		goldText.text = totalGold.ToString();
	}

	public void PlaySound(int audioIndex)
	{
		audioSource[audioIndex].clip = UIAudioClips[audioIndex];
		if (!audioSource[audioIndex].isPlaying) audioSource[audioIndex].Play();
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
