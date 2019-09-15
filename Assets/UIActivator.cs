using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIActivator : MonoBehaviour
{
	GameObject Player;
	public GameObject imagePanel;

	private Animator animator;
	bool showUI = false;

	[Header("UI Settings")]
	public TextMeshProUGUI titleText;
	public TextMeshProUGUI descriptionText;
	public string title;
	[TextArea(0,10)]
	public string description;

	void Awake()
	{
		animator = GetComponent<Animator>();
		Player = FindObjectOfType<PlayerCtrl>().gameObject;
	}

	void Start()
	{
		titleText.text = title;
		descriptionText.text = description;
		StartCoroutine(LookAtPlayer());
	}


	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.O))
		{
			ActivateUI();
		}
	}

	IEnumerator LookAtPlayer()
	{
		while(true)
		{
			imagePanel.transform.LookAt(new Vector3(Player.transform.position.x, imagePanel.transform.position.y, Player.transform.position.z));
			yield return null;
		}
	}

	public void ActivateUI()
	{
		showUI = !showUI;

		animator.SetBool("Show3DUI", showUI);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			ActivateUI();
			Debug.Log("Activate");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			ActivateUI();
			Debug.Log("Activate");
		}
	}
}
