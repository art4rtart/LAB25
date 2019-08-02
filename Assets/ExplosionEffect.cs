using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    // Core Information
    public float exploreRange;
    public LayerMask zombieMask;
    public Transform pivotPos;

    public void AfterExplosion()
    {
        Collider[] zombieInExploreRange = Physics.OverlapSphere(pivotPos.position, exploreRange, zombieMask);
        for (int i = 0; i < zombieInExploreRange.Length; i++)
        {
            Health health = zombieInExploreRange[i].GetComponent<Health>();

            if (health.hp > 0)
                health.DeathByBomb();

        }
    }
}
