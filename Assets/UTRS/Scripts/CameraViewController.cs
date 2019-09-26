using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewController : MonoBehaviour
{
	Camera cam;

	public Camera movingCamera;
	public float changeValue = 0f;
	public bool zoom = false;

	Transform cameraTransform;
	public Transform skyTarget;
	public Transform groundTarget;
	public float smoothness = 0.2f;

	public Transform currentTransform;
	public Transform ground_1;
	public Transform ground_2;

	bool isReadyForAction = true;
	bool isFirstTime;

	public Animator fadeAnim;

	void Start()
	{
		cam = Camera.main;
	}

	void Update()
	{
		if (UTRSMainMenu.Instance.barricadeClicked && isReadyForAction)
		{
			UTRSMainMenu.Instance.barricadeClicked = false;

			if (isFirstTime)
			{
				//BarigateController.Instance.enabled = !BarigateController.Instance.enabled;
				BarigateController.Instance.isMenuSelected = !BarigateController.Instance.isMenuSelected;
				BarigateController.Instance.selectedObject = null;
				BarigateController.Instance.Desk.SetActive(false);
				BarigateController.Instance.Box.SetActive(false);
				BarigateController.Instance.Fence.SetActive(false);
			}

			isFirstTime = true;

			zoom = !zoom;
			changeValue = 0;

			StopAllCoroutines();
			if (zoom) StartCoroutine(ZoomCamera(cam.transform, skyTarget));
			else StartCoroutine(ZoomCamera(cam.transform, UTRSManager.Instance.fpsCamera.transform));
		}

		if (zoom && Input.GetKeyDown(KeyCode.Escape))
		{
			zoom = false;
			StopAllCoroutines();
			if (zoom) StartCoroutine(ZoomCamera(cam.transform, skyTarget));
			else StartCoroutine(ZoomCamera(cam.transform, UTRSManager.Instance.fpsCamera.transform));
			UTRSMainMenu.Instance.anim.SetBool("FadeIn", true);
			StartCoroutine(UTRSManager.Instance.Blur(true));
			BarigateController.Instance.anim.SetBool("FadeIn", false);
			UTRSManager.Instance.MenuState = UTRSManager.CurrentMenu.Main;

			BarigateController.Instance.selectedObject = null;
			BarigateController.Instance.Desk.SetActive(false);
			BarigateController.Instance.Box.SetActive(false);
			BarigateController.Instance.Fence.SetActive(false);
		}


		if(Input.GetKeyDown(KeyCode.D) && isReadyForAction)
		{
			Transform current = null;
			Transform target = null;

			foreach (KeyValuePair<Transform, Transform> each in CurrentPlayerLocation())
			{
				current = each.Key;
				target = each.Value;
			}

			StartCoroutine(MoveCamera(current, target));
		}
	}

	Dictionary<Transform, Transform> CurrentPlayerLocation()
	{
		Dictionary<Transform, Transform> currentGround = new Dictionary<Transform, Transform>();

		if (this.transform.position.x > ground_1.transform.position.x - (ground_1.transform.localScale.x * 10 * 0.5f)
			&& this.transform.position.x < ground_1.transform.position.x + (ground_1.transform.localScale.x * 10 * 0.5f)
			&& this.transform.position.z < ground_1.transform.position.z + (ground_1.transform.localScale.z * 10 * 0.5f)
			&& this.transform.position.z < ground_1.transform.position.z + (ground_1.transform.localScale.z * 10 * 0.5f)
			) { currentGround.Add(ground_1, ground_2);  }

		if (this.transform.position.x > ground_2.transform.position.x - (ground_2.transform.localScale.x * 10 * 0.5f)
			&& this.transform.position.x < ground_2.transform.position.x + (ground_2.transform.localScale.x * 10 * 0.5f)
			&& this.transform.position.z < ground_2.transform.position.z + (ground_2.transform.localScale.z * 10 * 0.5f)
			&& this.transform.position.z < ground_2.transform.position.z + (ground_2.transform.localScale.z * 10 * 0.5f)
			) { currentGround.Add(ground_2, ground_1); }

		return currentGround;
	}

	IEnumerator ZoomCamera(Transform current, Transform target)
	{
		isReadyForAction = false;
		while (current.position != target.position && current.eulerAngles != target.eulerAngles)
		{
			current.position = Vector3.Lerp(current.position, target.position, changeValue);
			current.eulerAngles = Vector3.Lerp(current.eulerAngles, target.eulerAngles, changeValue);

			cam.transform.position = current.position;
			cam.transform.eulerAngles = current.eulerAngles;

			if (!zoom) { cam.orthographic = false; }

			changeValue += Time.deltaTime * smoothness;
			yield return null;
		}

		if (zoom)
		{
			fadeAnim.SetBool("Fade", true);
			while (true)
			{
				if (fadeAnim.GetCurrentAnimatorStateInfo(0).IsName("FadeIn") && fadeAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > .5f)
				{
					BarigateController.Instance.anim.SetBool("FadeIn", true);
					cam.orthographic = true;
					fadeAnim.SetBool("Fade", false);
					break;
				}
				yield return null;
			}
		}

		changeValue = 0;
		isReadyForAction = true;
	}

	IEnumerator MoveCamera(Transform current, Transform target)
	{
		isReadyForAction = false;
		Vector3 currentCamPos = new Vector3(current.position.x, skyTarget.position.y, current.position.z);
		Vector3 targetCamPos = new Vector3(target.position.x, skyTarget.position.y, target.position.z);

		Debug.Log(currentCamPos);
		while (currentCamPos != targetCamPos)
		{
			currentCamPos = Vector3.Lerp(currentCamPos, targetCamPos, changeValue);
			cam.transform.position = currentCamPos;

			changeValue += Time.deltaTime * smoothness;
			yield return null;
		}

		currentCamPos = Vector3.zero;
		changeValue = 0;
		isReadyForAction = true;
	}
}
