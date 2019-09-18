using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIActivator : MonoBehaviour
{
	GameObject Player;
	public GameObject imagePanel;

	private Animator animator;
	private AudioSource audioSource;
	bool showUI = false;
	bool isReady;
	bool isInteracting;

	[Header("Sound Settings")]
	public AudioClip[] audioClip;

	[Header("UI Settings")]
	public TextMeshProUGUI titleText;
	public TextMeshProUGUI descriptionText;
	public string title;
	[TextArea(0,10)]
	public string description;

	void Awake()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
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
		audioSource.Play();
		animator.SetBool("Show3DUI", showUI);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			isReady = true;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (Input.GetKeyDown(KeyCode.Q) && !isInteracting)
			{
				isInteracting = true;
				audioSource.clip = audioClip[0];
				ActivateUI();
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (isInteracting)
			{
				audioSource.clip = audioClip[1];
				isReady = isInteracting = false;
				ActivateUI();
			}
		}
	}
}
