using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PreviewObject : MonoBehaviour
{
	private static PreviewObject instance;
	public static PreviewObject Instance
	{
		get
		{
			if (instance != null)
				return instance;
			instance = FindObjectOfType<PreviewObject>();
			return instance;
		}
	}

	[Header("Object Rotate Settings")]
	public float rotateSpeed;
	Vector3 mPrevPos = Vector3.zero;
	Vector3 mPosDelta = Vector3.zero;

	[Header("Weapon Status")]
	public GameObject currentWeapon;
	public static float damage;
	public static float fireRate;
	public static float attackRange;
	public static int bullet;
	public static string weaponName;
	public static string weaponDescription;
	float damageValue;
	float fireRateValue;
	float bulletValue;
	float attackRangeValue;

	[Header("UI Reference")]
	public TextMeshProUGUI damageText;
	public TextMeshProUGUI fireRateText;
	public TextMeshProUGUI attackRangeText;
	public TextMeshProUGUI bulletText;
	public TextMeshProUGUI weaponNameText;
	public TextMeshProUGUI weaponDescriptionText;

	[Header("Animator Reference")]
	public Animator Canvas3DAnimator;
	public static Animator anim;

	[Header("Reference")]
	bool isPaused = false;
	bool isRotatable = false;
	public RectTransform contentRect;
	public Renderer blurRend;
	public Animator sceneFadeAnimator;
	public bool isNotUsingWeapon;

	void Start()
	{
		if (Canvas3DAnimator != null && anim == null) anim = Canvas3DAnimator;
		blurRend.sharedMaterial.SetFloat("_Size", 0);
		InitContentRectPosition();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Backspace))
		{
			ActivateOption();
		}

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

		// only item
		//isRotatable = (mousePos.x > -1f && mousePos.x < 3.8f && mousePos.y < 1.1f && mousePos.y > -1.7f) ? true : false;

		//if (Input.GetMouseButton(0) && isRotatable)
		//{
		//	mPosDelta = (Input.mousePosition - mPrevPos) * rotateSpeed;
		//	if(Vector3.Dot(currentWeapon.transform.up, Vector3.up) >= 0)
		//	{
		//		currentWeapon.transform.Rotate(currentWeapon.transform.up, -Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
		//	}
		//	else
		//	{
		//		currentWeapon.transform.Rotate(currentWeapon.transform.up, Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
		//	}
		//	currentWeapon.transform.Rotate(Camera.main.transform.right, Vector3.Dot(mPosDelta, Camera.main.transform.up), Space.World);
		//}

		mPrevPos = Input.mousePosition;
	}

	public void ShowWeaponStatus()
	{
		StartCoroutine(ShowWeaponByLerp());
	}

	void ActivateOption()
	{
		isPaused = !isPaused;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		this.gameObject.transform.GetChild(0).gameObject.SetActive(isPaused);
		UIManager.Instance.gameObject.SetActive(!isPaused);
		if(!isNotUsingWeapon) WeaponCtrl.Instance.enabled = !isPaused;
		PlayerCtrl.Instance.enabled = !isPaused;
		WeaponSway.Instance.enabled = !isPaused;
		sceneFadeAnimator.gameObject.SetActive(false);

		Canvas3DAnimator.SetBool("ShowMenu", isPaused);
		StopAllCoroutines();
		StartCoroutine(BlurBackground(isPaused));
	}

	IEnumerator BlurBackground(bool show)
	{
		float blurValue = 0;
		float targetValue = 0;

		if (show)
		{
			blurValue = 0f;
			targetValue = 20f;
			while (blurRend.sharedMaterial.GetFloat("_Size") < targetValue)
			{
				blurValue = Mathf.Clamp(blurValue += Time.deltaTime * 25f, 0, 20);
				blurRend.sharedMaterial.SetFloat("_Size", blurValue);
				yield return null;
			}
		}

		else
		{
			blurValue = 20f;
			targetValue = 0f;
			while (blurRend.sharedMaterial.GetFloat("_Size") > targetValue)
			{
				blurValue = Mathf.Clamp(blurValue -= Time.deltaTime * 25f, 0, 20);
				blurRend.sharedMaterial.SetFloat("_Size", blurValue);
				yield return null;
			}
		}
		yield return null;
	}

	IEnumerator ShowWeaponByLerp()
	{
		float lerpValue = 0;
		float lerpSpeed = 0.075f;
		damageValue = fireRateValue = bulletValue = attackRangeValue = 0;

		damageText.text = damageValue.ToString("N0");
		bulletText.text = bulletValue.ToString("N0");
		fireRateText.text = fireRateValue.ToString("N2") + "/S";
		attackRangeText.text = attackRangeValue.ToString("N0");

		yield return new WaitForSeconds(5.5f);

		while (damageValue != damage)
		{
			damageValue = Mathf.Lerp(damageValue, damage, lerpValue);
			damageText.text = damageValue.ToString("N0");

			fireRateValue = Mathf.Lerp(fireRateValue, fireRate, lerpValue);
			fireRateText.text = fireRateValue.ToString("N2") + "/S";

			attackRangeValue = Mathf.Lerp(attackRangeValue, attackRange, lerpValue);
			attackRangeText.text = attackRangeValue.ToString("N0");

			bulletValue = Mathf.Lerp(bulletValue, bullet, lerpValue);
			bulletText.text = bulletValue.ToString("N0");

			lerpValue += Time.deltaTime * lerpSpeed;
			yield return null;
		}
	}

	void InitContentRectPosition()
	{
		contentRect.localPosition = new Vector4(contentRect.localPosition.x, -1.05f, contentRect.localPosition.z);
	}

	public void TypeWeaponDescription()
	{
		StartCoroutine(TypeSentence(weaponDescription));
	}

	IEnumerator TypeSentence(string sentence)
	{
		weaponNameText.text = weaponName;
		weaponDescriptionText.text = "";

		for (int j = 0; j < sentence.Length; j++)
		{
			weaponDescriptionText.text += sentence[j];
			yield return null;
		}
	}
}
