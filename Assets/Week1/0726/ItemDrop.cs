using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
	public GameObject dropItem;
	public GameObject parent;
	GameObject item;

	float value;

	[Range(0,100)]
	public float percentage;

    void Start()
    {
		item = ((Random.Range(0, 100) <= percentage) ? Instantiate(dropItem, this.transform.position + Vector3.up * 1.5f, Quaternion.identity) : null);
		item.SetActive(false);
		if (item == null) return;
		item.transform.SetParent(parent.transform);
	}

	public void DropItem()
	{
		if (item == null) return;
		item.SetActive(true);
		item.transform.SetParent(null);
	}
}
