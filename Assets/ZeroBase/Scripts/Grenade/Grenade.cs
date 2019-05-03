using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
	public float delay = 3f;

	float countdown;
	bool hasExploded = false;

	public GameObject explosionEffect;
	public float radius;
	public float explosionForce;

	// Start is called before the first frame update
	void Start()
    {
		countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("asdsads");
		countdown -= Time.deltaTime;
		if(countdown <= 0f && !hasExploded)
		{
			Explode();
			hasExploded = true;
		}
    }

	void Explode()
	{
		Debug.Log("Exlpode");
		Instantiate(explosionEffect, this.transform.position, explosionEffect.transform.rotation);

		Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, radius);

		foreach (Collider nearbyObject in collidersToDestroy)
		{
			Destructible dest = nearbyObject.GetComponent<Destructible>();
			if(dest != null)
			{
				dest.Destroy();
			}
		}

		Collider[] collidersToMove = Physics.OverlapSphere(transform.position, radius);

		foreach (Collider nearbyObject in collidersToMove)
		{
			Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
			if (rb != null)
			{
				rb.AddExplosionForce(explosionForce, transform.position, radius);
			}
		}

		Destroy(gameObject);
	}
}
