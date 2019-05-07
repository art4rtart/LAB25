using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PostProcessing;

public class MedicalItem : MonoBehaviour
{

	[Header("Deafault")]
	ItemManager itemManager;
	public Animator animator;
	public GameObject HUD;
	float medicalItemActivateTime = 5f;
	float totalUseTime = 5f;

	[Header("Medical item stat info")]
	public float adrenalineUseTime;
	public float medicalKitUseTime;
	public string adrenalineUseMessage;
	public string medicalKitUseMessage;
	private bool adrenalineEffect;
	private bool medicalKitEffect;
	private float increaseSpeed = 10f;

	public Slider kitProgressbar;
	public TextMeshProUGUI useTimeText;
	public TextMeshProUGUI useMessageText;

	[Header("Medical item use effect")]
	public PostProcessingProfile ppProfile;
	public Beat beat;
	public PlayerCtrl playerCtrl;
	public WeaponCtrl weaponController;

	void Start()
    {
		SetFrameBlending(0f);
		itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
	}

    void Update()
    {
		// if (Input.GetMouseButtonDown(0) && itemManager.readyToUseAdrenaline && itemManager.adrenalineCount > 0)
		if (weaponController.useMedicalKit && itemManager.medicalKitCount > 0)
		{
			UseMedicalItem(medicalKitUseMessage, medicalKitUseTime);
			itemManager.medicalKitCount--;
		}

		//	if (Input.GetMouseButtonDown(0) && itemManager.readyToUseKit && itemManager.medicalKitCount > 0)
		if (weaponController.useAdrenaline && itemManager.adrenalineCount > 0)
		{
			UseMedicalItem(adrenalineUseMessage, adrenalineUseTime);
			itemManager.adrenalineCount--;
		}

		UseAnimation();
		UseAdrenaline(adrenalineEffect);
		UseMedicalKit(medicalKitEffect);

		kitProgressbar.value = Mathf.Floor(medicalItemActivateTime / totalUseTime * 1000) * 0.001f;
		useTimeText.text = Mathf.Clamp((Mathf.Floor(medicalItemActivateTime * 10) * 0.1f),0, totalUseTime).ToString();
	}

	void UseMedicalItem(string message, float time)
	{
		animator.SetBool("Use", true);
		useMessageText.text = message;
		medicalItemActivateTime = time;
		totalUseTime = time;
		itemManager.isHoldingSomething = false;
	}

	void UseAnimation()
	{
		// decrease time
		if (HUD.activeSelf && animator.GetCurrentAnimatorStateInfo(0).IsName("UseMedicalItemFadeIn") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
		{
			if (medicalItemActivateTime > 0) medicalItemActivateTime -= Time.deltaTime;
		}

		// activate item effect
		if (medicalItemActivateTime < 0)
		{
			if (totalUseTime == adrenalineUseTime)
			{
				adrenalineEffect = true;
			}

			else if (totalUseTime == medicalKitUseTime)
			{
				medicalKitEffect = true;
			}

			animator.SetBool("Use", false);

			if (animator.GetCurrentAnimatorStateInfo(0).IsName("UseMedicalItemFadeOut") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
			{
				weaponController.useAdrenaline = false;
				weaponController.useMedicalKit = false;
				medicalItemActivateTime = 5f;
				itemManager.isHoldingSomething = true;
			}
		}
	}

	public float targetMotionValue;
	public float continueTime;
	float blurMotionValue;
	public float blurMotionSpeed;
	float timeCount;
	
	float defaultHeartRateValue = 80f;
	float targetHeartRateValue = 155f;
	public Animator reloadAnim;
	float reloadAnimSpeed = 1f;

	void UseAdrenaline(bool _adrenalineEffect)
	{
		if (_adrenalineEffect)
		{
			timeCount += Time.deltaTime;

			if (timeCount < continueTime)
			{
				itemManager.currentHeartRate = Mathf.Clamp(itemManager.currentHeartRate += increaseSpeed * Time.deltaTime, defaultHeartRateValue, targetHeartRateValue);
				blurMotionValue = Mathf.Clamp(blurMotionValue += blurMotionSpeed * Time.deltaTime, 0f, targetMotionValue);

				playerCtrl.m_WalkSpeed = Mathf.Clamp(playerCtrl.m_WalkSpeed += blurMotionSpeed * 0.2f * Time.deltaTime, 5f, 10f);
				playerCtrl.m_RunSpeed = Mathf.Clamp(playerCtrl.m_RunSpeed += blurMotionSpeed * 0.2f * Time.deltaTime, 10f, 15f);

				reloadAnimSpeed = Mathf.Clamp(reloadAnimSpeed += blurMotionSpeed * Time.deltaTime, 1f, 1.5f);
				reloadAnim.SetFloat("ReloadSpeed", reloadAnimSpeed);
				beat.AdrenalineHeartBeatTrigger();
			}

			else
			{
				itemManager.currentHeartRate = Mathf.Clamp(itemManager.currentHeartRate -= increaseSpeed * Time.deltaTime, defaultHeartRateValue, targetHeartRateValue);
				blurMotionValue = Mathf.Clamp(blurMotionValue -= blurMotionSpeed * Time.deltaTime, 0f, targetMotionValue);

				playerCtrl.m_WalkSpeed = Mathf.Clamp(playerCtrl.m_WalkSpeed -= blurMotionSpeed * 0.4f * Time.deltaTime, 5f, 7f);
				playerCtrl.m_RunSpeed = Mathf.Clamp(playerCtrl.m_RunSpeed -= blurMotionSpeed * 0.4f * Time.deltaTime, 10f, 13f);
				reloadAnimSpeed = Mathf.Clamp(reloadAnimSpeed -= blurMotionSpeed * 0.4f * Time.deltaTime, 1f, 1.5f);
				reloadAnim.SetFloat("ReloadSpeed", reloadAnimSpeed);

				if (itemManager.currentHeartRate <= defaultHeartRateValue)
				{
					beat.DefaultHeartBeatTrigger();
					timeCount = 0;
					adrenalineEffect = false;
				}
			}
			SetFrameBlending(blurMotionValue);
		}
	}

	void UseMedicalKit(bool _medicalKitEffect)
	{
		if (_medicalKitEffect)
		{
			totalUseTime = 0;
			PlayerManager.hp += 40f;
			_medicalKitEffect = medicalKitEffect = false;
		}
	}

	void SetFrameBlending(float value)
	{
		MotionBlurModel.Settings motionblurSettings = ppProfile.motionBlur.settings;
		motionblurSettings.frameBlending = value;
		ppProfile.motionBlur.settings = motionblurSettings;
	}
}
