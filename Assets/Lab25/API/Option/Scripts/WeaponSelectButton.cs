using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelectButton : MonoBehaviour
{
	[Header("References")]
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI descriptionText;

	[Header("Weapon Descriptions")]
	public GameObject weapon;

	public string weaponName;

	[TextArea(0,10)]
	public string weaponDescription;
	[TextArea(0,10)]
	public string weaponDescriptionSub;

	public float weaponDamge;
	public float weaponFireRate;
	public float weaponAttackRange;
	public int wepaonBulletCount;

	[Header("Button Settings")]
	public static GameObject selectedButton;
	public Color normalColor;
	public Color targetColor;

	void OnEnable()
	{
		HideWeaponStatus();
	}

	void Start()
	{
		nameText.text = weaponName;
		descriptionText.text = weaponDescriptionSub;
	}

	public void ShowMenuStatus()
	{
		switch(ButtonAnimation.selectedMenuName)
		{
			case "Weapon":
				WeaponStatus();
				break;
			case "Item":
				ItemStatus();
				break;
			case "Mission":
				MissionStatus();
				break;
			case "Map":
				MapStatus();
				break;
			case "Option":
				OptionStatus();
				break;
		}
	}

	public void HideWeaponStatus()
	{
		this.GetComponent<Image>().color = new Color(normalColor.r, normalColor.g, normalColor.b, normalColor.a);
		this.GetComponent<Button>().interactable = true;
	}

	void WeaponStatus()
	{
		if (selectedButton != null)
		{
			selectedButton.GetComponent<Image>().color = new Color(normalColor.r, normalColor.g, normalColor.b, normalColor.a);
			selectedButton.GetComponent<Button>().interactable = true;
		}

		this.GetComponent<Image>().color = new Color(targetColor.r, targetColor.g, targetColor.b, targetColor.a);
		this.GetComponent<Button>().interactable = false;
		selectedButton = this.gameObject;

		PreviewObject.damage = weaponDamge;
		PreviewObject.fireRate = weaponFireRate;
		PreviewObject.attackRange = weaponAttackRange;
		PreviewObject.bullet = wepaonBulletCount;
		PreviewObject.weaponName = weaponName;
		PreviewObject.weaponDescription = weaponDescription;

		// animate
		PreviewObject.anim.SetTrigger("ShowWeaponStatus");
	}

	void ItemStatus()
	{
		PreviewObject.anim.SetTrigger("ShowItemStatus");
	}

	void MissionStatus()
	{
		PreviewObject.anim.SetTrigger("ShowMissionStatus");
	}

	void MapStatus()
	{
		PreviewObject.anim.SetTrigger("ShowMapStatus");
	}

	void OptionStatus()
	{
		PreviewObject.anim.SetTrigger("ShowOptionStatus");
	}
}
