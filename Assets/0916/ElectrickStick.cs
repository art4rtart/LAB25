using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrickStick : MonoBehaviour
{
	AudioSource audioSource;

	ParticleSystem ps;
	ParticleSystem subPs;
	bool isHoldingStick;

	void Start()
	{
		ps = GetComponent<ParticleSystem>();
		subPs = this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		if (WeaponCtrl.Instance.myWeapnType != WeaponCtrl.WEAPON.BONG) { ps.startSize = 0f; subPs.startSize = 0f; audioSource.Stop(); isHoldingStick = false; return; }
		else if (!isHoldingStick) { ps.startSize = 0.2f; subPs.startSize = 0.5f; audioSource.Play(); isHoldingStick = true; }

		if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
		{
			var trail = ps.trails;
			trail.mode = ParticleSystemTrailMode.Ribbon;
		}

		else
		{
			var trail = ps.trails;
			trail.mode = ParticleSystemTrailMode.PerParticle;
		}
	}
}
