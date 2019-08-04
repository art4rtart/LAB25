using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionScripts : MonoBehaviour
{
	[TextArea(3, 10)]
	public string[] sentences;
	[TextArea(3, 10)]
	public string[] subSentences;
	public int[] width;
	public int[] timerHeight;

	public TextMeshProUGUI playerMessage;
	public TextMeshProUGUI subMessage;

	public RectTransform missionRec;
	public RectTransform timerRec;

	public float timerCount = 5f;
	public float typeSpeed = 0f;
	[HideInInspector]
	public int index = 0;
	[HideInInspector]
	public int subIndex = 0;
	Animator animator;

	void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void Type()
	{
		playerMessage.text = "";
		TimerMessage.text = "";
		animator.SetBool("Type", true);
	}

	IEnumerator TypeSentence(string sentence)
	{
		playerMessage.text = "";

		for (int j = 0; j < sentence.Length; j++)
		{
			playerMessage.text += sentence[j];
			yield return new WaitForSeconds(typeSpeed);
		}

		StartCoroutine(Timer(timerCount));
	}

	public TextMeshProUGUI TimerMessage;

	IEnumerator Timer(float time)
	{
		float count = time;

		while(count >= 0)
		{
			count = Mathf.Clamp(count -= Time.deltaTime, 0, count);
			TimerMessage.text = string.Format("{0:F1}", count);

			if (count <= 0) {
				animator.SetBool("Type", false);
				index++;
				StopAllCoroutines();
			}

			yield return null;
		}
	}

	public void TpyeTrigger()
	{
		StartCoroutine(TypeSentence(sentences[index]));
		missionRec.sizeDelta = new Vector2(width[index], 150);
		timerRec.localPosition = new Vector2(0, timerHeight[index]);
	}
}
