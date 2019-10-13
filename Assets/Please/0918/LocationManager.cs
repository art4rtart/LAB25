using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocationManager : MonoBehaviour
{
	public static LocationManager Instance
	{
		get
		{
			if (instance != null)
				return instance;
			instance = FindObjectOfType<LocationManager>();
			return instance;
		}
	}
	private static LocationManager instance;

	private TextMeshProUGUI locationText;
	private Animator animator;

	void Start()
	{
		locationText = GetComponent<TextMeshProUGUI>();
		animator = GetComponent<Animator>();
	}

	public void SetLocationName(string name)
	{
		animator.SetTrigger("Change");
		StartCoroutine(ChangeName(name));
	}

	IEnumerator ChangeName(string name)
	{
		yield return new WaitForSeconds(.3f);
		locationText.text = name;
	}
}
