using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BombGage : MonoBehaviour
{
	public Slider slider;
	public RectTransform[] handle;
	public RectTransform gageHandler;
	public Animator stageAnimator;
	public TextMeshProUGUI BombInstallText;
	float number;

	float currentGage;
	bool isStart = false;
	int randomNumIndex = 0;

	float timeCount;

	bool printMessage = true;
	float addSpeed = 0.5f;

	bool isCoroutineStarted;
	float[] randomNum = new float[3];
	IEnumerator BombGageCoroutine;

    public Animator playerAnim;
    public bool hasBomb = false;
    public GameObject ScifiBomb;
    public WeaponCtrl myWeapon;


	void Awake()
	{
		BombGageCoroutine = BombInstall();
		installedBombCount = 0;
		BombInstallText.text = installedBombCount + " / 4 \ninstalled bomb";
	}

	void Start()
	{
		GenerateRandomPos();
	}

	void Update()
	{
		if(canInstall && Input.GetKeyDown(KeyCode.B) && !isCoroutineStarted)
		{
            AnimatorStateInfo info = playerAnim.GetCurrentAnimatorStateInfo(0);

            if (info.IsName("Idle") )
            {
                // player Anim
                playerAnim.SetTrigger("ReadyToBombSet");
                myWeapon.hasAK = false;
                myWeapon.hasAxe = false;
                // ui active

                // startCorutine
                BombGageCoroutine = BombInstall();
                StartCoroutine(BombGageCoroutine);
                isCoroutineStarted = true;
            }
            else
            {
                 if( info.IsName("weaponChange(AKtoBomb)"))
                {
                    // startCorutine
                    BombGageCoroutine = BombInstall();
                    StartCoroutine(BombGageCoroutine);
                    isCoroutineStarted = true;
                }
            }
		}
	}

	public static bool canInstall;
	IEnumerator BombInstall()
	{
		float correctCount = 0;
		PlayerManager.isHit = false;

		slider.GetComponent<Animator>().SetBool("BombGageFade", true);
		yield return new WaitForSeconds(.5f);
		
		// install sound play

		while (currentGage <= 4)
		{
			if (PlayerManager.isHit) break;

			currentGage = Mathf.Clamp(currentGage += Time.deltaTime * 1.5f * addSpeed, 0, 4);
			addSpeed += Time.deltaTime;

			slider.value = Mathf.Floor(currentGage * 100f) * 0.25f;

			if (slider.value > randomNum[randomNumIndex] + 5 && correctCount < 3)
			{
				slider.GetComponent<Animator>().SetBool("BombGageFade", false);
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
					slider.GetComponent<Animator>().SetBool("BombGageFade", false);
					break;
				}
			}

			if (correctCount >= 3f && slider.value == 100)
			{
				BombIsInstalled();
                playerAnim.SetTrigger("SuccesBombSet");
                Instantiate(ScifiBomb, playerAnim.transform.position + new Vector3(0f, 0.5f, 0f), transform.rotation, null);
                break;
			}

			yield return null;
		}

		isCoroutineStarted = false;
		slider.GetComponent<Animator>().SetBool("BombGageFade", false);

		yield return new WaitForSeconds(.5f);
		for (int i = 0; i < randomNum.Length; i++)
		{
			handle[i].GetComponent<Image>().color = Color.white;
		}

        playerAnim.ResetTrigger("ReadyToBombSet");
        playerAnim.ResetTrigger("SuccesBombSet");
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

	public static int installedBombCount = 0;
	public ZoneTriggers[] zoneTriggers;

	void BombIsInstalled()
	{
		if (installedBombCount < 4)
		{
			installedBombCount++;
			BombInstallText.text = installedBombCount + " / 4 \ninstalled bomb";
		}

		for(int i = 0; i < zoneTriggers.Length; i++)
		{
			if (zoneTriggers[i].isColliding)
			{
				zoneTriggers[i].changeZoneCircleRange();
				break;
			}
		}
	}
}
