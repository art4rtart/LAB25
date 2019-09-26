using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonAnimation : MonoBehaviour
{
	Animator animator;
	public static Button activateButton;
	public static Animator activateAnimator;
	public Image image;
	public Color normalColor;
	public Color targetColor;
	public string menuName;
	public static string selectedMenuName;

	public string stageName;

	public TextMeshProUGUI subMenuText1;
	public TextMeshProUGUI subMenuText2;

	void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void ButtonEnter()
	{
		animator.SetBool("Enter", true);
	}

	public void ButtonExit()
	{
		animator.SetBool("Enter", false);
	}

	public void ButtonClick()
	{
		InitButton();
		animator.SetBool("Click", true);
		// button click sound
		PreviewObject.anim.SetTrigger("Select");
		activateButton = this.GetComponent<Button>();
		activateAnimator = this.GetComponent<Animator>();

		activateButton.interactable = false;
		image.color = new Color(targetColor.r, targetColor.g, targetColor.b, targetColor.a);

		// menu check
		CheckMenuName();
	}

	public void UTRSButtonClick()
	{

	}

	void CheckMenuName()
	{
		switch(menuName)
		{
			case "Weapon":
				subMenuText1.text = "COLLECTION";
				subMenuText2.text = "WEAPON SELECT";
				PreviewObject.anim.SetTrigger("ShowWeaponMenu");
				PreviewObject.Instance.ShowWeaponStatus();

				break;
			case "Item":
				subMenuText1.text = "COLLECTION";
				subMenuText2.text = "ITEM CONDITIONS";
				PreviewObject.anim.SetTrigger("ShowItemMenu");
				break;
			case "Mission":
				subMenuText1.text = "";
				subMenuText2.text = "ACHIEVEMENT";
				PreviewObject.anim.SetTrigger("ShowMissionMenu");
				break;
			case "Map":
				subMenuText1.text = "";
				subMenuText2.text = stageName.ToString();
				PreviewObject.anim.SetTrigger("ShowMapMenu");
				break;
			case "Option":
				subMenuText1.text = "";
				subMenuText2.text = "GAME SETTINGS";
				PreviewObject.anim.SetTrigger("ShowOptionMenu");
				break;
		}
		selectedMenuName = menuName;
	}

	public void ButtonClickOff()
	{
		InitButton();
		PreviewObject.anim.SetTrigger("BackToMenu");

		PreviewObject.anim.ResetTrigger("Select");

		PreviewObject.anim.ResetTrigger("ShowWeaponStatus");
		PreviewObject.anim.ResetTrigger("ShowItemStatus");
		PreviewObject.anim.ResetTrigger("ShowMapStatus");
		PreviewObject.anim.ResetTrigger("ShowMissionStatus");
		PreviewObject.anim.ResetTrigger("ShowOptionStatus");

		this.GetComponent<Button>().interactable = false;
		this.GetComponent<Button>().interactable = true;

		// call weaponSelect
	}

	void InitButton()
	{
		if (activateButton == null) return;

		activateButton.GetComponent<Button>().interactable = false;
		activateButton.GetComponent<Button>().interactable = true;

		if (activateAnimator == null) return;
		activateAnimator.GetComponent<Animator>().SetBool("Click", false);

		activateButton.GetComponent<Image>().color = new Color(normalColor.r, normalColor.g, normalColor.b, normalColor.a);

		activateButton = null;
		activateAnimator = null;
	}
}
