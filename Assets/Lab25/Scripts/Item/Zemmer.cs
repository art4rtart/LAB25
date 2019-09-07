using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zemmer : MonoBehaviour
{
	public bool activate;
	int count;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
			UseZemmer();
	}
	public void UseZemmer()
	{
		if (count % 2 == 0)
			activate = true;
		else
			activate = false;
		count += 1;
	}
}
