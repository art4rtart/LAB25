using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTRSMainMenu : MonoBehaviour
{
	public static UTRSMainMenu Instance
	{
		get
		{
			if (instance != null)
				return instance;
			instance = FindObjectOfType<UTRSMainMenu>();
			return instance;
		}
	}
	private static UTRSMainMenu instance;

	public bool barricadeClicked = false;

	[HideInInspector] public Animator anim;


	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void ShowItemMenu()
	{
		UTRSManager.Instance.MenuState = UTRSManager.CurrentMenu.Item;

		anim.SetBool("FadeIn", false);
		Invoke("ShowItemMenuAfterFewSeconds", 1f);
	}

	public void ShowUpgradeMenu()
	{
		UTRSManager.Instance.MenuState = UTRSManager.CurrentMenu.Upgrade;

		anim.SetBool("FadeIn", false);
		Invoke("ShowUpgradeMenuAfterFewSeconds", 1f);
	}

	public void ShowBarricadeMenu()
	{
		UTRSManager.Instance.MenuState = UTRSManager.CurrentMenu.Barricade;

		//UTRSManager.Instance.mainCamera.transform.SetParent(null);

		barricadeClicked = true;
		anim.SetBool("FadeIn", false);
		StartCoroutine(UTRSManager.Instance.Blur(false));
		Invoke("ShowBarricadeMenuAfterFewSeconds", 1f);
	}

	void ShowItemMenuAfterFewSeconds()
	{
		UTRSManager.Instance.shopMenu.GetComponent<Animator>().SetBool("FadeIn", true);
	}

	void ShowUpgradeMenuAfterFewSeconds()
	{
		UTRSManager.Instance.upgradeMenu.GetComponent<Animator>().SetBool("FadeIn", true);
		UTRSManager.Instance.upgradeMenu.SetActive(true);
	}

	void ShowBarricadeMenuAfterFewSeconds()
	{
		UTRSManager.Instance.barricadeAddMenu.SetActive(true);
	}
}
