using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zemmer : MonoBehaviour
{
	public bool activate;
	int count;

	public void UseZemmer()
	{
		if (count % 2 == 0)
			activate = true;
		else
			activate = false;
		count += 1;
	}
}
