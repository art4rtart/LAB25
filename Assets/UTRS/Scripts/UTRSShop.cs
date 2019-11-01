using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UTRSShop : MonoBehaviour
{
	[Header("Sound Settings")]
	Animator anim;

	[Header("Price Settings")]
	public float medicalKitPrice;
	public float adrenalinePrice;
	public float akBulletPrice;
	public float scifiBulletPrice;
	public float armorPrice;

	public float akGunPrice;
	public float scifiGunPrice;

	public Animator goodAnim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) && (UTRSManager.Instance.MenuState != UTRSManager.CurrentMenu.Main && UTRSManager.Instance.MenuState != UTRSManager.CurrentMenu.Upgrade && UTRSManager.Instance.MenuState != UTRSManager.CurrentMenu.None))
		{
			anim.SetBool("FadeIn", false);
			UTRSMainMenu.Instance.anim.SetBool("FadeIn", true);
			UTRSManager.Instance.MenuState = UTRSManager.CurrentMenu.Main;
			UTRSManager.Instance.PlaySound(0);
		}
	}

	public void BuyMedicalKit()
	{
		medicalKitPrice = UTRSUpgrade.Instance.medicalKit.upgradePrice;
		if (UTRSManager.Instance.totalGold - medicalKitPrice >= 0)
			UTRSManager.Instance.totalGold -= medicalKitPrice;
		ItemManager.Instance.medicalKitCount++;
		UIManager.Instance.TextUpdate();
	}

	public void BuyAdrenaline()
	{
		adrenalinePrice = UTRSUpgrade.Instance.adrenaline.upgradePrice;
		if (UTRSManager.Instance.totalGold - adrenalinePrice >= 0)
			UTRSManager.Instance.totalGold -= adrenalinePrice;
		ItemManager.Instance.adrenalineCount++;
		UIManager.Instance.TextUpdate();
	}

	public void BuyAkBullet()
	{
		akBulletPrice = UTRSUpgrade.Instance.akBullet.upgradePrice;
		if (UTRSManager.Instance.totalGold - akBulletPrice >= 0)
			UTRSManager.Instance.totalGold -= akBulletPrice;
		WeaponCtrl.Instance.akBulletsTotal += 30;
		UIManager.Instance.TextUpdate();
	}

	public void BuySciFiBullet()
	{
		scifiBulletPrice = UTRSUpgrade.Instance.scifiBullet.upgradePrice;
		if (UTRSManager.Instance.totalGold - scifiBulletPrice >= 0)
			UTRSManager.Instance.totalGold -= scifiBulletPrice;
		WeaponCtrl.Instance.sciBulletsTotal += 150;
		UIManager.Instance.TextUpdate();
	}

	public void BuyArmor()
	{
		armorPrice = UTRSUpgrade.Instance.damageVest.upgradePrice;
		if (UTRSManager.Instance.totalGold - armorPrice >= 0)
			UTRSManager.Instance.totalGold -= armorPrice;
		PlayerManager.armor = Mathf.Clamp(PlayerManager.armor += 20, 0, 100f);
		UIManager.Instance.TextUpdate();
	}

	public void BuyAKGun()
	{
		akGunPrice = UTRSUpgrade.Instance.weaponAk.upgradePrice;
		if (UTRSManager.Instance.totalGold - akGunPrice >= 0)
		{
			UTRSManager.Instance.totalGold -= akGunPrice;
			goodAnim.SetTrigger("ElectricStickToAk");
			goodAnim.SetBool("isAkReady", true);
		}
	}

	public void BuySciFiGun()
	{
		scifiGunPrice = UTRSUpgrade.Instance.weaponScifi.upgradePrice;
		if (UTRSManager.Instance.totalGold - scifiGunPrice >= 0)
		{
			UTRSManager.Instance.totalGold -= scifiGunPrice;
			// scifi ready
		}
	}

	public void BuyMedic()
	{
		scifiGunPrice = UTRSUpgrade.Instance.weaponScifi.upgradePrice;
		if (UTRSManager.Instance.totalGold - scifiGunPrice >= 0)
		{
			UTRSManager.Instance.totalGold -= scifiGunPrice;
			// scifi ready
		}
	}

	public void BuyGuard()
	{
		scifiGunPrice = UTRSUpgrade.Instance.weaponScifi.upgradePrice;
		if (UTRSManager.Instance.totalGold - scifiGunPrice >= 0)
		{
			UTRSManager.Instance.totalGold -= scifiGunPrice;
			// scifi ready
		}
	}
}
