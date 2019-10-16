using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Effect
{
	public float currentEffectValue;
	public float targetEffectValue;
	public float upgradePrice;
	public float maxUpgradeValue;
	[HideInInspector] public bool isUpgradeAble = true;
}

public class UTRSUpgrade : MonoBehaviour
{
	public static UTRSUpgrade Instance
	{
		get
		{
			if (instance != null)
				return instance;
			instance = FindObjectOfType<UTRSUpgrade>();
			return instance;
		}
	}
	private static UTRSUpgrade instance;

	[Header("Item Upgrade")]
	public Effect medicalKit = new Effect();
	public Effect adrenaline = new Effect();
	public Effect damageVest = new Effect();
	public Effect akBullet = new Effect();
	public Effect scifiBullet = new Effect();

	[Header("Weapon Upgrade")]
	public Effect weaponAk = new Effect();
	public Effect weaponScifi = new Effect();

	[Header("Human Upgrade")]
	public Effect medic = new Effect();
	public Effect guard = new Effect();

	[Header("Barricade Upgrade")]
	public Effect desk = new Effect();
	public Effect box = new Effect();
	public Effect fence = new Effect();

	[Header("Passive Upgrade")]
	public Effect health = new Effect();
	public Effect armor = new Effect();
	public Effect gold = new Effect();

	Animator anim;

	WeaponCtrl weaponCtrl;

	[Header("TextMeshPro")]
	public TextMeshProUGUI medicalKitCurrentValueText;
	public TextMeshProUGUI medicalKitTargetValueText;
	public TextMeshProUGUI medicalKitUpgradePriceText;

	public TextMeshProUGUI adrenalineCurrentValueText;
	public TextMeshProUGUI adrenalineTargetValueText;
	public TextMeshProUGUI adrenalineUpgradePriceText;

	public TextMeshProUGUI armorCurrentValueText;
	public TextMeshProUGUI armorTargetValueText;
	public TextMeshProUGUI armorUpgradePriceText;

	public TextMeshProUGUI akBulletCurrentValueText;
	public TextMeshProUGUI akBulletTargetValueText;
	public TextMeshProUGUI akBulletUpgradePriceText;

	public TextMeshProUGUI sciFiBulletCurrentValueText;
	public TextMeshProUGUI ciFiBulletTargetValueText;
	public TextMeshProUGUI ciFiBulletUpgradePriceText;

	public TextMeshProUGUI akUpgradePriceText;
	public TextMeshProUGUI scifiUpgradePriceText;

	public 
	void Awake()
	{
		anim = GetComponent<Animator>();
		weaponCtrl = FindObjectOfType<WeaponCtrl>();
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && (UTRSManager.Instance.MenuState != UTRSManager.CurrentMenu.Main && UTRSManager.Instance.MenuState != UTRSManager.CurrentMenu.Item && UTRSManager.Instance.MenuState != UTRSManager.CurrentMenu.None))
		{
			anim.SetBool("FadeIn", false);
			UTRSMainMenu.Instance.anim.SetBool("FadeIn", true);
			UTRSManager.Instance.MenuState = UTRSManager.CurrentMenu.Main;
			UTRSManager.Instance.PlaySound(0);
		}
	}

	public void MedicalKitUpgrade()
	{

		medicalKit.upgradePrice *= 10f;
	}

	public void AdrenalineUpgrade()
	{
		UTRSManager.Instance.totalGold -= medicalKit.upgradePrice;

		if (medicalKit.targetEffectValue < medicalKit.maxUpgradeValue)
		{
			medicalKit.currentEffectValue = medicalKit.targetEffectValue;
			medicalKit.targetEffectValue *= 2f;
			medicalKit.upgradePrice *= 10f;
		}
		else medicalKit.isUpgradeAble = false;
	}

	public void DamageVestUpgrade()
	{
		medicalKit.upgradePrice *= 10f;
	}

	public void AkBulletKitUpgrade()
	{
		medicalKit.upgradePrice *= 10f;
	}

	public void SciFiBulletUpgrade()
	{
		medicalKit.upgradePrice *= 10f;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
	}

	public void AkWeaponUpgrade()
	{
		weaponAk.upgradePrice *= 2f;
		if(weaponCtrl.akFireRate > 0.01f) weaponCtrl.akFireRate -= 0.01f;
	}

	public void SciFiWeaponUpgrade()
	{

	}

	public void MedicKitUpgrade()
	{

	}

	public void GuardUpgrade()
	{

	}

	public void BarricadeDeskUpgrade()
	{

	}

	public void BarricadeBoxUpgrade()
	{

	}

	public void BarricadeFenceUpgrade()
	{

	}

	public void PassiveHealthUpgrade()
	{

	}

	public void PassiveArmorUpgrade()
	{

	}

	public void PassiveGoldUpgrade()
	{

	}
}
