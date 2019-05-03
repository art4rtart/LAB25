using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.PostProcessing;

public class Lobby : MonoBehaviour
{
	public Animator highlightMenuAnimator;
	public Animator menuAnimator;

	public PostProcessingProfile ppProfile;
	public GameObject menuCanvas;
	bool glowTrigger = false;

	[Header("UI")]
	public Button[] menu;
	public Button leftButton;
	public Button rightButton;

	public Texture[] stageSprites;
	public RawImage[] stageRawImage;

	public TextMeshProUGUI[] stageNameTextMeshPro;
	public string[] stagename;

	[Header("Index")]
	int[] index = new int[5];
	int spriteIndex;
	bool glowText = true;

	public GameObject blurRenderer;
	public GameObject QuitMesseageBox;

	[HideInInspector] public bool isSomethingClicked;
	[HideInInspector] public bool isSubMenuClosed;
	public int menuIndex;
	AudioManager audioManager;

	[Header("Cursor Settings")]
	public Texture2D cursorTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;


	void Awake()
	{
		audioManager = FindObjectOfType<AudioManager>();
	}

	void Start()
	{
		SetPostprocessing();
		RefreshMenu();
		blurRenderer.GetComponent<Renderer>().sharedMaterial.SetFloat("_Size", 0);
	}

	public void MoveLeftEvent()
	{
		spriteIndex = (spriteIndex + 1) % stageSprites.Length;
		RefreshMenu();
	}

	public void MoveRightButton()
	{
		spriteIndex = (spriteIndex - 1) % stageSprites.Length;
		RefreshMenu();
	}

	void RefreshMenu()
	{
		for (int i = 0; i < stageRawImage.Length; i++)
		{
			index[i] = Mathf.Abs(spriteIndex) + i;
			stageRawImage[i].texture = stageSprites[index[i] % 5];
			stageNameTextMeshPro[i].text = stagename[index[i] % 5];
		}
	}

	void SetPostprocessing()
	{
		BloomModel.Settings bloomSettings = ppProfile.bloom.settings;
		bloomSettings.bloom.intensity = 2f;
		ppProfile.bloom.settings = bloomSettings;
	}

	public void LobbyActiveEvent()
	{
		menuCanvas.SetActive(true);
	}

	//void OnMouseEnter()
	//{
	//	Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
	//}

	//void OnMouseExit()
	//{
	//	Cursor.SetCursor(null, Vector2.zero, cursorMode);
	//}
}
