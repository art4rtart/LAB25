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
	public WeaponCtrl weaponController;
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
    int maxBullet = 30;
    int totalBullet = 360;
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

    void Start()
    {
        currentWeaponImage.sprite = weaponImage[0];
    }

    void Update()
    {
        TextUpdate();
        SpriteUpdate();
        DeveloperMode();
        MissionUpdate();
    }

    void MissionUpdate()
    {
        if (isMissionStart)
        {
            missionAnimator.SetBool("MissionStart", isMissionStart);

            if (missionAnimator.GetCurrentAnimatorStateInfo(0).IsName("MissionStart")
                && missionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
                missionTime -= Time.deltaTime;
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
    }

    void TextUpdate()
    {
		healthProgressbar.value = ItemManager.currentHealth;
        armorProgressbar.value = ItemManager.currentArmor;
		bulletProgressbar.value = Mathf.Floor((float)WeaponCtrl.currentBullets / (float)maxBullet * 100f) * 0.01f;

        healthText.text = Mathf.Clamp(Mathf.Floor(ItemManager.currentHealth), 0, itemManager.totalHealth).ToString();
        armorText.text = Mathf.Clamp(Mathf.Floor(ItemManager.currentArmor), 0, itemManager.totalArmor).ToString();
        heratRateText.text = Mathf.Clamp(Mathf.Floor(itemManager.currentHeartRate), 0, itemManager.totalHeartRate).ToString();

        // bullet Update
        bulletCountText.text = Mathf.Clamp(WeaponCtrl.currentBullets, 0, maxBullet).ToString();
        totalBulletText.text = totalBullet.ToString();

        // item Update
        kitCountText.text = itemManager.medicalKitCount.ToString();
        adrenalineCountText.text = itemManager.adrenalineCount.ToString();
        grenadeCountText.text = itemManager.grenadeCount.ToString();

        // mission Update
        int minutes = Mathf.FloorToInt(missionTime / 60F);
        int seconds = Mathf.FloorToInt(missionTime - minutes * 60);
        string time = string.Format("{0:0}:{1:00}", minutes, seconds);

        missionTimeText.text = time.ToString();
        missionMesseageText.text = missionMessage;
    }

    void SpriteUpdate()
    {
        if (itemManager.isHoldingWeapon && changeWeaponImage)
        {
            noWeaponText.enabled = false;
            currentWeaponImage.sprite = weaponImage[itemManager.weaponIndex];
            changeWeaponImage = false;
        }

        interactAnimator.SetBool("FadeIn", isPointingItem);     // f key update
    }

    void DeveloperMode()
    {
		if (weaponController.isReloaded)
		{
			totalBullet -= (maxBullet - WeaponCtrl.currentBullets);
			WeaponCtrl.currentBullets = maxBullet;
			bulletProgressbar.value = 1f;
			weaponController.isReloaded = false;
		}

		if (Input.GetKeyDown(KeyCode.M))
            isMissionStart = true;

        if (Input.GetKeyDown(KeyCode.C))
            isMissionComplete = true;
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
