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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
		{
			Hit();
		}
	}

	void Hit()
	{
		int index = Random.Range(0, sprites.Length);
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >0.75f)
			image.sprite = sprites[index];
		anim.SetTrigger("Hit");
	}
}
