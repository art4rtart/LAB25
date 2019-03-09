using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MedicalItem : MonoBehaviour
{

	[Header("Deafault")]
	ItemManager itemManager;
	public Animator animator;
	public GameObject HUD;
	float medicalItemActivateTime = 5f;
	float totalUseTime = 5f;

	[Header("Medical Item stat Info")]
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

	void Start()
    {
		itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
	}

    void Update()
    {
		if (Input.GetMouseButtonDown(0) && itemManager.readyToUseAdrenaline && itemManager.adrenalineCount > 0)
		{
			UseMedicalItem(adrenalineUseMessage, adrenalineUseTime);
			itemManager.adrenalineCount--;
		}

		if (Input.GetMouseButtonDown(0) && itemManager.readyToUseKit && itemManager.medicalKitCount > 0)
		{
			UseMedicalItem(medicalKitUseMessage, medicalKitUseTime);
			itemManager.medicalKitCount--;
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
				medicalItemActivateTime = 5f;
				itemManager.isHoldingSomething = true;
			}
		}
	}

	void UseAdrenaline(bool _adrenalineEffect)
	{
		if (_adrenalineEffect)
		{
			float targetHeartRate = 150f; 
			if (itemManager.currentHeartRate < targetHeartRate)
				itemManager.currentHeartRate += increaseSpeed * Time.deltaTime;
			else
				_adrenalineEffect = false;
		}
	}

	void UseMedicalKit(bool _medicalKitEffect)
	{
		if (_medicalKitEffect)
		{
			totalUseTime = 0;
			PlayerManager.hp += 40f;
			Debug.Log("HI");
			_medicalKitEffect = medicalKitEffect = false;
		}
	}
}
