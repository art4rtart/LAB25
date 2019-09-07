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
	public Crosshair crosshair;
	//public WeaponCtrl weaponController;

	[Header("Item Count")]
	[HideInInspector] public int medicalKitCount;
	[HideInInspector] public int adrenalineCount;
	[HideInInspector] public int grenadeCount;
	[HideInInspector] public int beakerCount;

	[Header("Interact Rage")]
	public float range = 10f;
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
	public static float currentHealth = 100f;
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
	public bool hasHearter;
	public GameObject elevator;
	public ScriptController scriptController;

	[Header("Cross Hair")]
	public TextMeshProUGUI zombieNameText;
	public TextMeshProUGUI zombieHealthText;
	bool isTargetLocked;
	public CoreExploder coreExploder;

    public static void SetPlayerStat()
    {
        currentHealth = PlayerManager.hp;
        currentArmor = PlayerManager.armor;
    }

	void Awake()
	{
		initializeData();
	}

    void Update()
	{
		//currentHeartRate =
		// -----------------------------------------------

		currentHealth = Mathf.Clamp(Mathf.Clamp(PlayerManager.hp, 0, 100f), 0, totalHealth);
		currentArmor = Mathf.Clamp(PlayerManager.armor, 0, totalArmor);

		RaycastHit hit;
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
		{
			if(hit.transform.CompareTag("Infectee"))
			{
				zombieNameText.text = hit.transform.name;
				zombieHealthText.text = Mathf.Clamp(hit.transform.GetComponent<Health>().hp, 0, hit.transform.GetComponent<Health>().hp).ToString();

				if(!isTargetLocked)
				{
					crosshair.LockCrosshair();
					isTargetLocked = true;
				}
			}

			else
			{
				if (isTargetLocked)
				{
					crosshair.LockCrosshair();
					isTargetLocked = false;
				}
			}

			if (hit.transform.CompareTag("MissionObject") && quest.openGateMission)
			{
				uiManager.isPointingItem = true;

				if (hit.transform.name.Equals( "ElevatorSensor"))
				{
					itemNameText.text = "ELEVATOR BUTTON";

					if (Input.GetKeyDown(KeyCode.F))
					{
						if (uiManager.isMissionStart) return;

						uiManager.missionTime = 40f;
						uiManager.missionMessage = "SURVIVE UNTIL ELEVATOR ARRIVES";
						uiManager.isMissionStart = true;

						uiManager.isPointingItem = false;
						pointTrigger = false;
						if(elevator != null) elevator.GetComponent<Animator>().SetTrigger("PressElevator");
						// scriptController.startGenerator = true;
					}
				}

				else if (hit.transform.name .Equals( "PowerButton"))
				{
					itemNameText.text = "POWER GENERATER";
					if(coreExploder != null) coreExploder.isReadyToOverPower = true;
				}

				else if (hit.transform.name .Equals( "AutomaticSensor"))
				{
					itemNameText.text = "GATE SENSOR";

					if (Input.GetKeyDown(KeyCode.F))
					{
						hit.transform.GetComponent<Sensor>().OpenGate();
					}
				}

				else if (hit.transform.name .Equals( "ElevatorCloseButton"))
				{
					itemNameText.text = "CLOSE ELEVATOR";
					if (Input.GetKeyDown(KeyCode.F))
					{
						uiManager.isPointingItem = false;
						pointTrigger = false;


						if (elevator != null)
						{
							if (BombGage.Instance.installedBombCount .Equals( 4))
							{
								elevator.GetComponent<Animator>().SetTrigger("CloseElevator");
								player.transform.SetParent(elevator.GetComponent<Elevator>().elevator.transform);
								player.GetComponent<CharacterController>().enabled = false;
							}
							else
								Debug.Log("You have to install a bomb!!");
						}
					}
				}

				else if (hit.transform.name .Equals( "GateButtonTypeB"))
				{
					itemNameText.text = "GATE SENSOR";

					if (Input.GetKeyDown(KeyCode.F))
					{
						missionScript.GetComponent<Animator>().SetTrigger("Finish");
						uiManager.missionTime = 40f;
						uiManager.missionMessage = "VIRUS ROOM GATE IS CLOSING";
						uiManager.isMissionStart = true;

						uiManager.isPointingItem = false;
						pointTrigger = false;
						quest.OpenAndCloseGate(hit.transform.GetChild(0).gameObject);
					}
				}

				else	if (hit.transform.name .Equals( "GateButton"))
				{
					itemNameText.text = "CARD KEY SENSOR";

					if (Input.GetKeyDown(KeyCode.F) && hasCardKey )
                    {
						missionScript.GetComponent<Animator>().SetTrigger("Finish");
						missionScript.Type(); // need conditions
						uiManager.isPointingItem = false;
						pointTrigger = false;
						quest.OpenGate(hit.transform.GetChild(0).gameObject);
                    }
                }
				pointTrigger = true;
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
				if (!hit.transform.CompareTag("Item") && !hit.transform.CompareTag("MissionObject"))
				{
					if (item != null)
					{
						item.GetComponent<GlowObject>().ChangeToDefaultColor();
						item = null;
					}

					pointTrigger = false;
					isInteracting = false;
					if(coreExploder != null) coreExploder.isReadyToOverPower = false;
					uiManager.isPointingItem = false;
					return;
				}
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
				PlayerManager.armor += 100f;
				break;

			case "Medical Kit":
				medicalKitCount++;
				break;

			case "Adrenaline Syringe":
				missionScript.Type();
				adrenalineCount++;
				break;

			case "Grenade":
				grenadeCount += 100;
				break;

			case "7.76mm Bullet Crate":
				uiManager.totalBullet += 30;
                if( WeaponCtrl.Instance.myWeapnType.Equals(WeaponCtrl.WEAPON.AKM ))
                    WeaponCtrl.Instance.akBulletsTotal += 30;
                else if (WeaponCtrl.Instance.myWeapnType.Equals( WeaponCtrl.WEAPON.SCI_FI))
                    WeaponCtrl.Instance.sciBulletsTotal += 30;
                break;

			case "Biometrics Goggle":
				StartCoroutine(uiManager.FadeIn());
				isWearingHelmet = true;
				break;

			case "Gate Card Key":
				hasCardKey = true;
				break;

			case "Hearter":
				hasHearter = true;
				break;

			case "Jammer":
				missionScript.Type();
				hasJammer = true;
				break;

			case "Ward":
				hasWard = true;
				break;

			case "Beaker":
				beakerCount += 1;
				break;

			case "Sci-Fi Machine Gun":
				WeaponCtrl.Instance.anim.SetBool("toScifi", true);
				break;
		}
        UIManager.Instance.TextUpdate();

		if (item.gameObject.CompareTag("Item")) { item.gameObject.SetActive(false); WeaponCtrl.Instance.audioSource.PlayOneShot(WeaponCtrl.Instance.itemGetSound); }
	}

	void initializeData()
	{
		medicalKitCount = 0;
		adrenalineCount = 0;
		grenadeCount = 0;
		beakerCount = 5;
		isUsingWard = false;
		isUsingHearter = false;
		currentHealth = 100f;
		currentArmor = 100f;
		currentHeartRate = 90f;
	}

	bool hasWard;
	public static bool hasCardKey = false;
	bool hasJammer;
}
