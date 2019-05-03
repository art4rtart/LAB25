using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDistance : MonoBehaviour
{
	public Text text;
	public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(Distance());
    }

    // Update is called once per frame
	IEnumerator Distance()
	{
		while (true)
		{
			this.transform.localScale = new Vector3(-1, 1, 1);
			transform.LookAt(player.transform, Vector3.up);
			text.text = string.Format("{0:#.#}", Vector3.Distance(this.transform.position, player.transform.position)) + " M";
			yield return new WaitForSeconds(0.5f);
		}
    }
}
