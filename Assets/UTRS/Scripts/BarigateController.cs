using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarigateController : MonoBehaviour
{
	public static BarigateController Instance
	{
		get
		{
			if (instance != null)
				return instance;
			instance = FindObjectOfType<BarigateController>();
			return instance;
		}
	}
	private static BarigateController instance;

	[Header("Editor")]
	public GameObject Desk;
	public GameObject Box;
	public GameObject Fence;

	[Header("Prefab")]
	public GameObject DeskPrefab;
	public GameObject BoxPrefab;
	public GameObject FencePrefab;

	[Header("Price")]
	public float DeskPrice;
	public float BoxPrice;
	public float fencePrice;

	public GameObject selectedObject;

	public bool isMenuSelected;

	[HideInInspector] public Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		if (isMenuSelected && selectedObject != null)
		{
			selectedObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			selectedObject.transform.position = new Vector3(selectedObject.transform.position.x, selectedObject.transform.localScale.y * 0.5f, selectedObject.transform.position.z);

			if (Input.GetMouseButtonDown(0) && !isCollidingWithUI)
			{
				if(selectedObject.GetComponent<CheckBarigateCollision>().isAddable) Instantiate(InstallObject(selectedObject.name), selectedObject.transform.position, Quaternion.identity);
			}

			if (!selectedObject.activeSelf) selectedObject.SetActive(true);
		}
	}

	bool isCollidingWithUI = false;
	public void UICollideEnter()
	{
		isCollidingWithUI = true;
	}

	public void UICollideExit()
	{
		isCollidingWithUI = false;
	}


	public void BuyDesk()
	{
		selectedObject = null;
		selectedObject = Desk;

		Box.SetActive(false);
		Fence.SetActive(false);

		isMenuSelected = true;
	}

	public void BuyBox()
	{
		selectedObject = null;
		selectedObject = Box;

		Desk.SetActive(false);
		Fence.SetActive(false);

		isMenuSelected = true;
	}

	public void BuyFence()
	{
		selectedObject = null;
		selectedObject = Fence;

		Desk.SetActive(false);
		Box.SetActive(false);

		isMenuSelected = true;
	}

	GameObject InstallObject(string name)
	{
		GameObject barigate = null;
		switch (name)
		{
			case "Desk":
				barigate = DeskPrefab;
				break;
			case "Box":
				barigate = BoxPrefab;
				break;
			case "Fence":
				barigate = FencePrefab;
				break;
		}

		return barigate;
	}
}
