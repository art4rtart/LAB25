﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
	[Header("Default")]
	public Camera fpsCam;
	public GameObject player;
	public UIManager uiManager;

	[Header("Item Count")]
	[HideInInspector] public int medicalKitCount;
	[HideInInspector] public int adrenalineCount;
	[HideInInspector] public int grenadeCount;

	[Header("Interact Rage")]
	public float range = 100f;
	public float itemGetRange = 5f;

	[Header("Item Swap")]
	[HideInInspector] public bool readyToUseGun;
	[HideInInspector] public bool readyToUseKit;
	[HideInInspector] public bool readyToUseAdrenaline;
	[HideInInspector] public bool readyToUseGrenade;

	[Header("Current Item")]
	[HideInInspector] public int weaponIndex;
	[HideInInspector] public bool isWearingHelmet;
	[HideInInspector] public bool isHoldingWeapon;
	[HideInInspector] public bool isHoldingSomething = true;

	[Header("Player Stat")]
	public static float currentHealth = 30f;
	public float totalHealth = 100f;
	public static float currentArmor = 100f;
	public float totalArmor = 100f;
	public float currentHeartRate = 90f;
	public float totalHeartRate = 150f;

	// wait
	bool gainArmor;
	public float lerpSpeed;

	public static float targetHealth;
	public static float targetArmor;

	float lerpTime;
	float armorLerpTime;

	[HideInInspector] public static bool takeDamage;
	void Update()
	{
		// define player stat here ---------------------
		currentHealth = PlayerManager.hp;
		currentArmor = PlayerManager.armor;
		//currentHeartRate =
	   // -----------------------------------------------

	   currentHealth = Mathf.Clamp(currentHealth, 0, totalHealth);
		currentArmor = Mathf.Clamp(currentArmor, 0, totalArmor);

		// Lerp Player Status
		//if (Input.GetButtonDown("Fire1"))
		//{
		//	targetHealth = currentHealth - 10f;
		//	takeDamage = true;
		//}

		//if (Input.GetButtonDown("Fire2"))
		//{
		//	targetHealth = currentHealth + 10f;
		//	takeDamage = true;
		//}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Debug.Log("I am Holding a Gun");

			readyToUseGun = true;
			readyToUseKit = false;
			readyToUseAdrenaline = false;
			readyToUseGrenade = false;
		}

		if (Input.GetKeyDown(KeyCode.Alpha2) && isHoldingSomething)
		{
			Debug.Log("I am Holding a Kit");

			readyToUseGun = false;
			readyToUseKit = true;
			readyToUseAdrenaline = false;
			readyToUseGrenade = false;

			isHoldingSomething = true;
		}

		if (Input.GetKeyDown(KeyCode.Alpha3) && isHoldingSomething)
		{
			Debug.Log("I am Holding a Adrenalin");

			readyToUseGun = false;
			readyToUseAdrenaline = true;
			readyToUseKit = false;
			readyToUseGrenade = false;

			isHoldingSomething = true;
		}

		if (Input.GetKeyDown(KeyCode.Alpha4) && isHoldingSomething)
		{
			Debug.Log("I am Holding a Grenade");

			readyToUseGun = false;
			readyToUseKit = false;
			readyToUseAdrenaline = false;
			readyToUseGrenade = true;

			isHoldingSomething = true;
		}

		// Point Item
		RaycastHit hit;
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
		{
			if (hit.transform.CompareTag("Item"))
			{
				if (Vector3.Distance(player.transform.position, hit.transform.position) < itemGetRange)
					uiManager.isPointingItem = true;
			}

			else
			{
				uiManager.isPointingItem = false;
			}
		}

		// Get Item
		if (Input.GetKeyDown(KeyCode.F) && uiManager.isPointingItem)
			GetItem(hit.transform);

		GainArmor(gainArmor);
		HealthManager(takeDamage);
	}

	void GetItem(Transform item)
	{
		switch (item.transform.name)
		{
			case "Knife":
				isHoldingWeapon = uiManager.changeWeaponImage = true;
				weaponIndex = 1;
				break;

			case "Axe":
				isHoldingWeapon = uiManager.changeWeaponImage = true;
				weaponIndex = 2;
				break;

			case "Glock":
				isHoldingWeapon = uiManager.changeWeaponImage = true;
				weaponIndex = 3;
				break;

			case "Sniper":
				isHoldingWeapon = uiManager.changeWeaponImage = readyToUseGun = true;
				weaponIndex = 4;
				break;

			case "Vest":
				gainArmor = true;
				targetArmor = currentArmor + 40f;
				break;

			case "Kit":
				medicalKitCount++;
				break;

			case "Adrenaline":
				adrenalineCount++;
				break;

			case "Grenade":
				grenadeCount++;
				break;

			case "Helmet":
				StartCoroutine(uiManager.FadeIn());
				isWearingHelmet = true;
				break;
		}

		item.gameObject.SetActive(false);
	}

	void GainArmor(bool _gainArmor)
	{
		if (_gainArmor)
		{
			currentArmor = Mathf.Lerp(currentArmor, targetArmor, armorLerpTime);
			armorLerpTime += Time.deltaTime * lerpSpeed;

			if (Mathf.Floor(currentArmor) == Mathf.Floor(targetArmor))
			{
				currentArmor = targetArmor = Mathf.Floor(currentArmor);
				_gainArmor = false;
				armorLerpTime = 0;
			}
		}
	}

	void HealthManager(bool _takeDamage)
	{
		if (_takeDamage)
		{
			currentHealth = Mathf.Lerp(currentHealth, targetHealth, lerpTime);
			lerpTime += Time.deltaTime * lerpSpeed;

			if (Mathf.Floor(currentHealth) == Mathf.Floor(targetHealth))
			{
				currentHealth = targetHealth = Mathf.Floor(currentHealth);
				_takeDamage = false;
				lerpTime = 0;
			}
		}
	}
}
