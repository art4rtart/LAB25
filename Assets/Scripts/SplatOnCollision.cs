using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatOnCollision : MonoBehaviour
{

    //public ParticleSystem splatterParticles;
    //public ParticleDecalPool dropletDecalPool;

    //List<ParticleCollisionEvent> collisionEvents;

    public ParticleSystem splatterParticles;
    public List<ParticleCollisionEvent> collisionEvents;
    public MemoryPool bloodParticlePool;
    public GameObject bloodParticlePrefab;
    private Transform fireTraceParent;

    private void Awake()
    {
        fireTraceParent = GameObject.Find("ObjectManager").transform;
    }
    void Start()
    {
        splatterParticles = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        bloodParticlePool = new MemoryPool();
        bloodParticlePool.Create(bloodParticlePrefab, 100, fireTraceParent);
    }

    int index = 0;
    bool tempBool = true;

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = splatterParticles.GetCollisionEvents(other, collisionEvents);

        if (numCollisionEvents <= collisionEvents.Count)
        {
            if (index != numCollisionEvents)
            {
                if (index == collisionEvents.Count - 1 && tempBool)
                {
                    for (int i = 0; i < collisionEvents.Count; i++)
                    {
                        //StartCoroutine(BloodEffect(new Vector3(collisionEvents[i].intersection.x, 0.05f, collisionEvents[i].intersection.z)));
                        var bloodParticle = (GameObject)Instantiate(bloodParticlePrefab, collisionEvents[i].intersection, bloodParticlePrefab.transform.rotation);
                        bloodParticle.transform.position = new Vector3(bloodParticle.transform.position.x, 0.05f, bloodParticle.transform.position.z);
                    }
                    tempBool = false;
                }
                index++;
            }
        }
    }

    IEnumerator BloodEffect(Vector3 pos)
    {
        GameObject bloodParticleTemp;
        bloodParticleTemp = bloodParticlePool.NewItem();

        if (bloodParticleTemp)
        {
            bloodParticleTemp.transform.position = pos;
            bloodParticleTemp.transform.SetParent(fireTraceParent);
        }
        yield return new WaitForSeconds(2.5f);
        bloodParticlePool.RemoveItem(bloodParticleTemp, bloodParticlePrefab, fireTraceParent);
    }
}
