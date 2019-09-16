using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrickStick : MonoBehaviour
{
	ParticleSystem ps;

	void Start()
	{
		ps = GetComponent<ParticleSystem>();
	}

	void Update()
	{
		if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
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
