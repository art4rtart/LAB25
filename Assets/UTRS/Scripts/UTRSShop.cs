using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UTRSShop : MonoBehaviour
{
	[Header("Sound Settings")]
	AudioSource audioSource;
	Animator anim;
	public AudioClip[] purchaseSound;

	[Header("Price Settings")]
	public float medicalKitPrice;
	public float adrenalinePrice;
	public float akBulletPrice;
	public float scifiBulletPrice;
	public float armorPrice;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
	}

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) && (UTRSManager.Instance.MenuState != UTRSManager.CurrentMenu.Main && UTRSManager.Instance.MenuState != UTRSManager.CurrentMenu.Upgrade && UTRSManager.Instance.MenuState != UTRSManager.CurrentMenu.None))
		{
			anim.SetBool("FadeIn", false);
			//StartCoroutine(UTRSManager.Instance.Blur(false));
			UTRSMainMenu.Instance.anim.SetBool("FadeIn", true);
			UTRSManager.Instance.MenuState = UTRSManager.CurrentMenu.Main;
		}
	}

	public void BuyMedicalKit()
	{
		//audioSource.clip = purchaseSound[0];
		audioSource.Play();
		UTRSManager.Instance.totalGold -= medicalKitPrice;
	}

	public void BuyAdrenaline()
	{
		//audioSource.clip = purchaseSound[1];
		audioSource.Play();
		UTRSManager.Instance.totalGold -= adrenalinePrice;
	}

	public void BuyAkBullet()
	{
		//audioSource.clip = purchaseSound[2];
		audioSource.Play();
		UTRSManager.Instance.totalGold -= akBulletPrice;
	}

	public void BuySciFiBullet()
	{
		//audioSource.clip = purchaseSound[3];
		audioSource.Play();
		UTRSManager.Instance.totalGold -= scifiBulletPrice;
	}

	public void BuyArmor()
	{
		//audioSource.clip = purchaseSound[4];
		audioSource.Play();
		UTRSManager.Instance.totalGold -= armorPrice;
	}
}
