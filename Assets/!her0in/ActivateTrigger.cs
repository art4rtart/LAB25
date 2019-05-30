using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrigger : MonoBehaviour
{
    UIManager uiManager;
    public GameObject activateObject;
    public bool enableObject;

    public bool changePlaceName;
    public string placeName;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
    }

    void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
            if (enableObject && !activateObject.activeSelf) activateObject.SetActive(true);
            if (changePlaceName) uiManager.placeText.text = placeName;
        }
	}
}
