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

	public bool[] isSceneLocked;

	[Header("UI")]
	public Button[] menu;
	public Button leftButton;
	public Button rightButton;

	public Texture[] stageSprites;
	public RawImage[] stageRawImage;
	public Texture[] stageLockSprites;
	public RawImage[] stageLockRawImage;

	public TextMeshProUGUI[] stageNameTextMeshPro;
	public string[] stagename;

	[Header("Index")]
	int[] index = new int[5];
	public static int spriteIndex;
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
		spriteIndex = 0;
		menuIndex = 0;
	}

	void Start()
	{
		SetPostprocessing();
		RefreshMenu();
		blurRenderer.GetComponent<Renderer>().sharedMaterial.SetFloat("_Size", 0);
	}

	public void MoveLeftEvent()
	{
		audioManager.Play("DefaultClickSound");
		spriteIndex = (spriteIndex + 1) % stageSprites.Length;
		RefreshMenu();
	}

	public void MoveRightButton()
	{
		audioManager.Play("DefaultClickSound");
		spriteIndex = (spriteIndex - 1) % stageSprites.Length;
		RefreshMenu();
	}

	void RefreshMenu()
	{
		if (Mathf.Abs(spriteIndex) == 0) leftButton.enabled = false;
		else leftButton.enabled = true;

		if (Mathf.Abs(spriteIndex) == 4) rightButton.enabled = false;
		else rightButton.enabled = true;

		for (int i = 0; i < stageRawImage.Length; i++)
		{
			index[i] = Mathf.Abs(spriteIndex) + i;
			stageRawImage[i].texture = stageSprites[index[i] % 5];
			stageLockRawImage[i].texture = stageLockSprites[index[i] % 5];
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

	void OnMouseEnter()
	{
		Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
	}

	void OnMouseExit()
	{
		Cursor.SetCursor(null, Vector2.zero, cursorMode);
	}
}
