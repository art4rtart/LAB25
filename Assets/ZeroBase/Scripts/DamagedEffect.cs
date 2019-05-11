using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagedEffect : MonoBehaviour
{
	Animator anim;
	public Sprite[] sprites;
	Image image;

    void Awake()
    {
		anim = GetComponent<Animator>();
		image = GetComponent<Image>();
	}

	public void Hit()
	{
		int index = Random.Range(0, sprites.Length);

		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f)
			image.sprite = sprites[index];

		if(ItemManager.currentHealth > 0) anim.SetTrigger("Hit");
	}
}
