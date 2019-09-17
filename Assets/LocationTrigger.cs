using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
	public string locationName;
	LocationManager locationManager;

	void Start()
	{
		locationManager = FindObjectOfType<LocationManager>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == null) return;

		if (other.gameObject.CompareTag("Location"))
		{
			locationManager.SetLocationName(locationName);
		}
	}
}
