using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombGage : MonoBehaviour
{
	public Slider slider;
	public RectTransform[] handle;
	public RectTransform gageHandler;
	float number;

	float currentGage;

	float[] randomNum = new float[3];
	IEnumerator BombGageCoroutine;

	void Awake()
	{
		BombGageCoroutine = BombInstall();
	}

	void Start()
	{
		GenerateRandomPos();
	}

	bool isStart = false;
	int randomNumIndex = 0;

	float timeCount;

	bool printMessage = true;
	float addSpeed = 0.5f;


	int bKeyCount;

	bool isCoroutineStarted;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.B) && !isCoroutineStarted)
		{
			// player Anim

			// ui active

			// startCorutine
			BombGageCoroutine = BombInstall();
			StartCoroutine(BombGageCoroutine);
			isCoroutineStarted = true;
		}


		//if (PlayerManager.isHit) return;

		//if (Input.GetKeyDown(KeyCode.Y)) { isStart = true; }
		//if (!isStart) return;
	}

	IEnumerator BombInstall()
	{
		float correctCount = 0;
		slider.GetComponent<Animator>().SetBool("BombGageFade", true);
		yield return new WaitForSeconds(1f);
		
		// install sound play

		while (currentGage <= 4)
		{
			if (PlayerManager.isHit) break;

			currentGage = Mathf.Clamp(currentGage += Time.deltaTime * addSpeed, 0, 4);
			addSpeed += Time.deltaTime;

			slider.value = Mathf.Floor(currentGage * 100f) * 0.25f;

			if (slider.value > randomNum[randomNumIndex] + 5 && correctCount < 3)
			{
				break;
			}

			if (Input.GetKeyDown(KeyCode.B))
			{
				if (randomNum[randomNumIndex] - 5 < slider.value && slider.value < randomNum[randomNumIndex] + 5)
				{
					// change color
					handle[randomNumIndex].GetComponent<Image>().color = Color.red;

					// correct sound play
					if (randomNumIndex < randomNum.Length - 1) randomNumIndex++;
					correctCount++;
				}

				else
				{
					break;
				}
			}

			if (correctCount >= 3f && slider.value == 100) break;
			yield return null;
		}

		isCoroutineStarted = false;
		slider.GetComponent<Animator>().SetBool("BombGageFade", false);

		yield return new WaitForSeconds(.5f);
		for (int i = 0; i < randomNum.Length; i++)
		{
			handle[i].GetComponent<Image>().color = Color.white;
		}

		randomNumIndex = 0;
		slider.value = 0;
		addSpeed = 0;
		currentGage = 0;
		GenerateRandomPos();
		StopCoroutine(BombGageCoroutine);
	}

	void GenerateRandomPos()
	{
		for (int i = 0; i < randomNum.Length; i++)
		{
			randomNum[i] = Random.Range(5 + (i * 30), 35 + (i * 30));
			slider.handleRect = handle[i];
			handle[i].anchoredPosition = new Vector2(0, 0);
			handle[i].sizeDelta = new Vector2(20, 0);
			handle[i].localScale = new Vector3(1, 1, 1);
			slider.value = randomNum[i];
		}

		slider.handleRect = gageHandler;
		slider.value = 0;
	}
}
