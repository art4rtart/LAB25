using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
	public ItemManager itemManager;

	public float throwForce = 40f;
	public GameObject grenadePrefab;
	public Transform grenadeThrowPos;
	public GameObject grenadeTrail;
	public LineRenderer lineRenderer;

	public Material material;
	public float fadeSpeed = 0.5f;
	bool readyToThrow;
	float alpha;

	public Animator playerAnim;
	public WeaponCtrl myWeapon;
	//&& itemManager.grenadeCount > 0
	void Update()
	{
		material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);

		if (itemManager.readyToUseGrenade)
		{
			playerAnim.SetTrigger("ReadyToThrow");
			myWeapon.hasAK = false;
			readyToThrow = lineRenderer.enabled = true;
		}
	    if (Input.GetMouseButtonUp(0) && readyToThrow)
		{
			ThrowGrenade();
			readyToThrow = false;
			StartCoroutine("ThrowCup");
			alpha = 0;
			itemManager.readyToUseGrenade = false;
			playerAnim.ResetTrigger("ReadyToThrow");
		}

		if (readyToThrow)
		{
			if (alpha < 1)
				alpha += fadeSpeed * Time.deltaTime;
		}

		else
		{
			if (alpha > 0)
				alpha -= fadeSpeed * Time.deltaTime;
		}

		if (alpha < 0.01f)
			lineRenderer.enabled = false;
	}

	void ThrowGrenade()
	{
		itemManager.grenadeCount--;
        UIManager.Instance.TextUpdate();
        GameObject grenade = Instantiate(grenadePrefab, grenadeThrowPos.position, transform.rotation);
		Rigidbody rb = grenade.GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
		rb.AddForce(transform.up * throwForce / 8, ForceMode.VelocityChange);
	}
	
	IEnumerator ThrowCup()
	{
		playerAnim.SetBool("isThrow", true);
		yield return new WaitForSeconds(1.0f);
		playerAnim.SetBool("isThrow", false);
		myWeapon.hasAK = true;
	}
}
