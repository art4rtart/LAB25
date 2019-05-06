using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
	public GameObject gate;

	public IEnumerator CloseElevator()
	{
		float lerp = 0;

		gate.GetComponent<Animator>().SetBool("GateOpen", false);

		yield return new WaitForSeconds(2f);

		while (this.transform.position.y < 20f)
		{
			this.transform.position = new Vector3(this.transform.position.x, Mathf.Lerp(this.transform.position.y, 20f, lerp), this.transform.position.z);
			lerp += Time.deltaTime * Time.deltaTime;
			yield return null;
		}

		yield return null;
	}

	public IEnumerator MoveElevator()
	{
		float time = 2f;

		while(time >= 0)
		{
			time -= Time.deltaTime;
			yield return null;
		}

		float lerp = 0;

		while (this.transform.position.y > 4f)
		{
			this.transform.position = new Vector3(this.transform.position.x, Mathf.Lerp(this.transform.position.y, 4f, lerp), this.transform.position.z);
			lerp += Time.deltaTime * Time.deltaTime;
			yield return null;

			float y = Mathf.Floor(this.transform.position.y * 100) / 100;
			if (y <= 4f) break;
		}

		gate.GetComponent<Animator>().SetBool("GateOpen", true);

		StopAllCoroutines();
		yield return null;
	}
}
