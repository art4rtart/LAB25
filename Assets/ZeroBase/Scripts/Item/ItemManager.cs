using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

	[Header("Special Item")]
	[HideInInspector] public bool isUsingWard;
	[HideInInspector] public bool isUsingHearter;

	[Header("Player Stat")]
	public static float currentHealth = 30f;
	public float totalHealth = 100f;
	public static float currentArmor = 100f;
	public float totalArmor = 100f;
	public float currentHeartRate = 90f;
	public float totalHeartRate = 150f;

	bool pointTrigger;
	Transform item;
	public TextMeshProUGUI itemNameText;
	[HideInInspector] public static bool takeDamage;
	bool isInteracting = false;

	// her0in new
	public MissionScripts missionScript;
	public Quest quest;

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

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Debug.Log("I am Holding a Adrenalin");

			readyToUseGun = false;
			readyToUseAdrenaline = true;
			readyToUseKit = false;
			readyToUseGrenade = false;

			isHoldingSomething = true;
		}

		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			Debug.Log("I am Holding a Grenade");

			readyToUseGun = false;
			readyToUseKit = false;
			readyToUseAdrenaline = false;
			readyToUseGrenade = true;

			isHoldingSomething = true;
		}

		RaycastHit hit;

		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
		{
			if (hit.transform.CompareTag("MissionObject") && quest.openGateMission)
			{
				if (hit.transform.name == "GateButton")
				{
					if (Input.GetKeyDown(KeyCode.F))
						quest.OpenGate();
				}
			}

			if (hit.transform.CompareTag("Item") && !isInteracting)
			{
				isInteracting = true;
				item = hit.transform;

				if (Vector3.Distance(player.transform.position, hit.transform.position) < itemGetRange)
				{
					if (!pointTrigger)
					{
						item.GetComponent<GlowObject>().ChangeToTargetColor();
						uiManager.isPointingItem = true;
						itemNameText.text = hit.transform.name;
						pointTrigger = true;
					}
				}
			}

			else
			{
				if (!hit.transform.CompareTag("Item"))
				{
					if (item != null)
					{
						item.GetComponent<GlowObject>().ChangeToDefaultColor();
						item = null;
					}
					pointTrigger = false;
					isInteracting = false;
					uiManager.isPointingItem = false;
					return;
				}

				//else if (item != null)
				//{
				//	Debug.Log("HI");
				//	pointTrigger = false;
				//	item.GetComponent<GlowObject>().ChangeToDefaultColor();
				//	item = null;
				//}

				//uiManager.isPointingItem = false;
				//isInteracting = false;
			}
		}

		// Get Item
		if (Input.GetKeyDown(KeyCode.F) && uiManager.isPointingItem)
			GetItem(hit.transform);
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

			case "Damage Protection Vest":
				PlayerManager.armor += 40f;
				break;

			case "Medical Kit":
				medicalKitCount++;
				break;

			case "Adrenaline Syringe":
				adrenalineCount++;
				break;

			case "Grenade":
				grenadeCount += 100;
				break;

			case "Biometrics Goggle":
				StartCoroutine(uiManager.FadeIn());
				isWearingHelmet = true;
				break;
		}

		item.gameObject.SetActive(false);
		missionScript.Type();
	}
}
