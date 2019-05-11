using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
	public GameObject gate;
	public GameObject player;
	public Animator fadeAnim;
	public UIManager uiManager;
	public string nextSceneName;

	public IEnumerator CloseElevator()
	{
		float lerp = 0;

		gate.GetComponent<Animator>().SetBool("GateOpen", false);
		player.transform.SetParent(this.gameObject.transform);

		yield return new WaitForSeconds(2f);

		player.GetComponent<CharacterController>().enabled = false;
		fadeAnim.SetTrigger("SceneEnd");

		while (this.transform.position.y < 15f)
		{
			this.transform.position = new Vector3(this.transform.position.x, Mathf.Lerp(this.transform.position.y, 20f, lerp), this.transform.position.z);
			lerp += Time.deltaTime * Time.deltaTime;
			yield return null;
		}

		SceneManager.LoadScene(nextSceneName);
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

		while (this.transform.position.y > 3.5f)
		{
			this.transform.position = new Vector3(this.transform.position.x, Mathf.Lerp(this.transform.position.y, 3.5f, lerp), this.transform.position.z);
			lerp += Time.deltaTime * Time.deltaTime;
			yield return null;

			float y = Mathf.Floor(this.transform.position.y * 100) / 100;
			if (y <= 3.5f) break;
		}

		gate.GetComponent<Animator>().SetBool("GateOpen", true);
		uiManager.isMissionComplete = true;
		StopAllCoroutines();
		yield return null;
	}
}
