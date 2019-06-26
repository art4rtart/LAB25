using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public static Particle Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<Particle>();
            return instance;
        }
    }

    private static Particle instance;

    // Prefabs
    public GameObject hitSparkPrefab;
    public GameObject hitHolePrefab;
    public GameObject bulletCasingPrefab;
    public GameObject bloodParticlePrefab;

    // ObjectPool
    public static MemoryPool bulletHolePool = new MemoryPool();
    public static MemoryPool flarePool = new MemoryPool();
    public static MemoryPool bulletCasingPool = new MemoryPool();
    public static MemoryPool bloodParticlePool = new MemoryPool();

    private Transform fireTraceParent;
    public Transform bulletCasingPoint;

    private void Awake()
    {
        fireTraceParent = GameObject.Find("ObjectManager").transform;
    }

    private void Start()
    {
        bulletHolePool.Create(hitHolePrefab, 6, fireTraceParent);
        flarePool.Create(hitSparkPrefab, 6, fireTraceParent);
        bulletCasingPool.Create(bulletCasingPrefab, 11, fireTraceParent);
        bloodParticlePool.Create(bloodParticlePrefab, 100, fireTraceParent);
    }
    public IEnumerator BulletEffect()
    {
        GameObject bulletCasingObject;
        bulletCasingObject = bulletCasingPool.NewItem();
        Quaternion randomQuaternion = new Quaternion(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(180, 360f), 1);

        if (bulletCasingObject)
        {
            bulletCasingObject.transform.position = transform.TransformPoint(transform.InverseTransformPoint(bulletCasingPoint.position) + new Vector3(0.4f, 0, 0.3f));
            bulletCasingObject.transform.localScale = new Vector3(2, 2, 2);
            bulletCasingObject.transform.SetParent(fireTraceParent);
            bulletCasingObject.transform.localRotation = randomQuaternion;
            //bulletCasingObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(Random.Range(0.01f, 0.05f), Random.Range(0.01f, 0.05f), Random.Range(-0.03f, 0.03f)));
            bulletCasingObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(Random.Range(50f, 100f), Random.Range(50f, 100f), Random.Range(-30f, 30f)));
        }
        yield return new WaitForSeconds(1f);

        bulletCasingPool.RemoveItem(bulletCasingObject, bulletCasingPrefab, fireTraceParent);
    }

    public IEnumerator FireEffect(Vector3 pos, Quaternion rot)
    {
        GameObject bulletHole, flare;
        bulletHole = bulletHolePool.NewItem();
        flare = flarePool.NewItem();

        if (bulletHole)
        {
            bulletHole.transform.position = pos;
            bulletHole.transform.SetParent(fireTraceParent);
            bulletHole.transform.rotation = rot;

            flare.transform.position = pos;
            flare.transform.SetParent(fireTraceParent);
            flare.transform.rotation = rot;
        }
        yield return new WaitForSeconds(0.5f);
        flarePool.RemoveItem(flare, hitSparkPrefab, fireTraceParent);

        yield return new WaitForSeconds(0.5f);
        bulletHolePool.RemoveItem(bulletHole, hitHolePrefab, fireTraceParent);
    }

    public IEnumerator BloodEffect(Vector3 pos)
    {
        GameObject bloodParticleTemp;
        bloodParticleTemp = bloodParticlePool.NewItem();
        if (bloodParticleTemp)
        {
            bloodParticleTemp.transform.position = pos;
            bloodParticleTemp.transform.SetParent(fireTraceParent);
        }
        yield return new WaitForSeconds(1.0f);
        bloodParticlePool.RemoveItem(bloodParticleTemp, bloodParticlePrefab, fireTraceParent);
    }

    void OnApplicationQuit()
    {
        bloodParticlePool.Dispose();
        bulletHolePool.Dispose();
        flarePool.Dispose();
        bulletCasingPool.Dispose();
    }
}
