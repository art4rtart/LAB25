using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	public Animator interactAnimator;
	public Animator missionAnimator;
	public float range = 100f;
	public float fireRate = 15f;
	float nextTimeToFire = 0f;
	bool takeDamge;
	private float lerpTime;
	public float sliderLerpSpeed = 2f;

	[Header("Default")]
	public Camera fpsCam;
	public GameObject HUD;
	//public WeaponCtrl weaponController;
	public ItemManager itemManager;

	[Header("Helmet")]
	public Image helmetFadeImage;
	public float fadeSpeed = 2f;

	[Header("Player UI")]
	public Slider healthProgressbar;
	public Slider armorProgressbar;
	public TextMeshProUGUI healthText;
	public TextMeshProUGUI armorText;
	public TextMeshProUGUI heratRateText;
	public TextMeshProUGUI placeText;

	[Header("Mission UI")]
	public TextMeshProUGUI missionTimeText;
	public TextMeshProUGUI missionMesseageText;
	public bool isMissionStart;
	public bool isMissionComplete;
	public float missionTime;
	public float newMissionTime;
	public string missionMessage;
	public string newMissionMessage;

	[Header("Weapon UI")]
	public int maxBullet;
	public int totalBullet;
	public int bulletsPerMagazine = 30;
	float bulletlerpTime;
	bool fired;
	public TextMeshProUGUI noWeaponText;
	public TextMeshProUGUI bulletCountText;
	public TextMeshProUGUI totalBulletText;
	public Slider bulletProgressbar;
	public Sprite[] weaponImage;
	public Image currentWeaponImage;

	[Header("Item UI")]
	public TextMeshProUGUI kitCountText;
	public TextMeshProUGUI adrenalineCountText;
	public TextMeshProUGUI grenadeCountText;
	[HideInInspector] public bool isPointingItem;
	[HideInInspector] public bool changeWeaponImage;

	public static UIManager Instance
	{
		get
		{
			if (instance != null)
				return instance;
			instance = FindObjectOfType<UIManager>();
			return instance;
		}
	}

	private static UIManager instance;

	void Start()
	{
		currentWeaponImage.sprite = weaponImage[0];
		TextUpdate();
	}

	void Update()
	{
		//TextUpdate();
		SpriteUpdate();
		//DeveloperMode();
		MissionUpdate();

		if (Input.GetKeyDown(KeyCode.M))
			isMissionStart = true;

		if (Input.GetKeyDown(KeyCode.C))
			isMissionComplete = true;
	}

	public void MissionUpdate()
	{
		if (isMissionStart)
		{
			missionAnimator.SetBool("MissionStart", isMissionStart);

			if (missionAnimator.GetCurrentAnimatorStateInfo(0).IsName("MissionStart") && missionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
				missionTime = Mathf.Clamp(missionTime -= Time.deltaTime, 0f, 100f);
		}

		if (isMissionComplete)
		{
			isMissionStart = false;
			missionAnimator.SetBool("MissionStart", isMissionStart);

			if (missionAnimator.GetCurrentAnimatorStateInfo(0).IsName("MissionComplete")
				&& missionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
			{
				missionTime = newMissionTime;
				missionMessage = newMissionMessage;
				isMissionComplete = false;
			}
		}

		// mission Update
		int minutes = Mathf.FloorToInt(missionTime / 60F);
		int seconds = Mathf.FloorToInt(missionTime - minutes * 60);
		string time = string.Format("{0:0}:{1:00}", minutes, seconds);
		missionTimeText.text = time.ToString();
		missionMesseageText.text = missionMessage;
	}

	public void TextUpdate()
	{
		healthProgressbar.value = ItemManager.currentHealth / 100f;
		armorProgressbar.value = ItemManager.currentArmor / 100f;
        if (WeaponCtrl.Instance.myWeapnType == WeaponCtrl.WEAPON.AKM)
            bulletProgressbar.value = Mathf.Floor((float)WeaponCtrl.Instance.akCurrentBullets / (float)bulletsPerMagazine * 100f) * 0.01f;

        else if (WeaponCtrl.Instance.myWeapnType == WeaponCtrl.WEAPON.SCI_FI)
            bulletProgressbar.value = Mathf.Floor((float)WeaponCtrl.Instance.sciCurrentBullets / (float)150 * 100f) * 0.01f;

		// healthText.text = Mathf.Clamp(Mathf.Floor(ItemManager.currentHealth), 0, itemManager.totalHealth).ToString();
		//armorText.text = Mathf.Clamp(Mathf.Floor(ItemManager.currentArmor), 0, itemManager.totalArmor).ToString();
		heratRateText.text = Mathf.Clamp(Mathf.Floor(itemManager.currentHeartRate), 0, itemManager.totalHeartRate).ToString();

        // bullet Update
        if (WeaponCtrl.Instance.myWeapnType == WeaponCtrl.WEAPON.AKM)
            bulletCountText.text = Mathf.Clamp(WeaponCtrl.Instance.akCurrentBullets, 0, bulletsPerMagazine).ToString();
        else if (WeaponCtrl.Instance.myWeapnType == WeaponCtrl.WEAPON.SCI_FI)
            bulletCountText.text = Mathf.Clamp(WeaponCtrl.Instance.sciCurrentBullets, 0, 150).ToString();

        totalBulletText.text = totalBullet.ToString();

		// item Update
		kitCountText.text = itemManager.medicalKitCount.ToString();
		adrenalineCountText.text = itemManager.adrenalineCount.ToString();
		grenadeCountText.text = itemManager.grenadeCount.ToString();
	}

	public void SpriteUpdate()
	{
		if (itemManager.isHoldingWeapon && changeWeaponImage)
		{
			noWeaponText.enabled = false;
			currentWeaponImage.sprite = weaponImage[itemManager.weaponIndex];
			changeWeaponImage = false;
		}

		interactAnimator.SetBool("FadeIn", isPointingItem);     // f key update
	}

	public void DeveloperMode()
	{
        if (WeaponCtrl.Instance.myWeapnType == WeaponCtrl.WEAPON.AKM)
        {
            WeaponCtrl.Instance.akCurrentBullets += WeaponCtrl.Instance.bulletsToReload;
            totalBullet = WeaponCtrl.Instance.akBulletsTotal;
        }

        else if (WeaponCtrl.Instance.myWeapnType == WeaponCtrl.WEAPON.SCI_FI)
        { 
            WeaponCtrl.Instance.sciCurrentBullets += WeaponCtrl.Instance.bulletsToReload;
            totalBullet = WeaponCtrl.Instance.sciBulletsTotal;
        }

        bulletProgressbar.value = 1f;
	}

	public IEnumerator FadeIn()
	{
		for (float i = 0; i <= 1; i += fadeSpeed * Time.deltaTime)
		{
			helmetFadeImage.color = new Color(helmetFadeImage.color.r, helmetFadeImage.color.g, helmetFadeImage.color.b, i);
			yield return null;
		}
		HUD.SetActive(true);
		yield return new WaitForSeconds(.1f);
		StartCoroutine(FadeOut());
	}

	IEnumerator FadeOut()
	{
		for (float i = 1; i >= 0; i -= fadeSpeed * Time.deltaTime)
		{
			helmetFadeImage.color = new Color(helmetFadeImage.color.r, helmetFadeImage.color.g, helmetFadeImage.color.b, i);
			yield return null;
		}
	}
}

