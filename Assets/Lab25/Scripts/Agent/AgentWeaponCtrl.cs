using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentWeaponCtrl : MonoBehaviour
{
    // Weapon Specification
    public int bulletsPerMag;
    public int bulletsTotal;
    public int currentBullets;
    public float range;
    public float fireRate;
    public float shotFireRate;
    public float flameFireRate;
    private Vector3 originalPos;
    public float accuracy;
    private float originalAccuracy;
    public int damage;
    public int shotGunDamage;
    public int flameDamage;
    public int medicalKitCount;

    // Parameters
    private float fireTimer;
    private float shotFireTimer;
    private float flameFireTimer;

    private bool isReloading = false;
    private bool isHealing = false;
    private bool isRunning;

    // Sounds
    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip shotGunSound;
    public AudioClip reloadSound;
    public AudioClip drawSound;

    // References

    //private Animator anim;
    public ParticleSystem muzzleFlash;
    private CharacterController characterController;

    // Prefabs
    //public GameObject hitSparkPrefab;
    //public GameObject hitHolePrefab;
    //public GameObject bloodParticlePrefab;

    // Animations
    private int hashAttack = Animator.StringToHash("isAttack");
    private int hashHeal = Animator.StringToHash("isHeal");
    public Animator anim;

    public Transform shootPos;

    public LabAgent labAgent;
    public FlameThrower flameThrower;
    private int playerMask;
    private readonly string playerStr = "Player";
    private void Awake()
    {
        //fireTraceParent = GameObject.Find("ObjectManager").transform;

    }
    private void Start()
    {
        //bulletHolePool.Create(hitHolePrefab, 6, fireTraceParent);
        //flarePool.Create(hitSparkPrefab, 6, fireTraceParent);

        //bloodParticlePool.Create(bloodParticlePrefab, 100, fireTraceParent);
        characterController = GetComponentInParent<CharacterController>();
        playerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));
        anim = transform.root.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //Debug.DrawRay(shootPos.transform.position, shootPos.transform.forward * 100f, Color.red);

        if (fireTimer < fireRate)
            fireTimer += Time.fixedDeltaTime;
 
        //Vector3 forward = transform.TransformDirection(Vector3.forward) * 199f;
        //Debug.DrawRay(transform.position + Vector3.up, transform.forward * 300, Color.green);
    }

    public void Fire()
    {
  
        if (fireTimer < fireRate)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(shootPos.transform.position, shootPos.transform.forward, out hit, 100f, playerMask))
        {

            Health health = hit.transform.GetComponent<Health>();

            if (health && health.hp > 0)
            {
			
                health.ApplyDamage(43, hit.transform.InverseTransformPoint(hit.point));
                if (health.hp <= 0)
                {
                    StartCoroutine(ParticleManager.Instance.BloodTraceEffect(hit.transform.position));
                }


                StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));

            }
            else
            {
                if (!hit.transform.tag.Equals(playerStr))
                {
                    StartCoroutine(ParticleManager.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
                }
                else
                {
                    StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));
                }
            }
        }


        currentBullets--;
        fireTimer = 0.0f;
        anim.CrossFadeInFixedTime("Shoot", 0.01f);
        audioSource.PlayOneShot(shootSound);    //shoot sound
        muzzleFlash.Play();
    }

    public void FireShotGun()
    {
        if (fireTimer < shotFireRate)
        {
            return;
        }

        RaycastHit hit;
        for (int i = 0; i < 5; ++i)
        {
            if (Physics.Raycast(shootPos.transform.position, shootPos.transform.forward, out hit, 100f, playerMask))
            {
                Health health = hit.transform.GetComponent<Health>();

                if (health && health.hp > 0)
                {
                    health.ApplyDamage(43, hit.transform.InverseTransformPoint(hit.point));
                    if (health.hp <= 0)
                    {
                        StartCoroutine(ParticleManager.Instance.BloodTraceEffect(hit.transform.position));
                    }


                    StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));

                }
                else
                {
                    if (!hit.transform.tag.Equals(playerStr))
                    {
                        StartCoroutine(ParticleManager.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
                    }
                    else
                    {
                        StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));
                    }
                }
            }
        }
        currentBullets--;
        fireTimer = 0.0f;

        anim.CrossFadeInFixedTime("Shoot", 0.01f);
        audioSource.PlayOneShot(shotGunSound);    //shoot sound
        muzzleFlash.Play();
    }

    public void FireFlameThrower()
    {
        if (fireTimer < flameFireRate)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(shootPos.transform.position, shootPos.transform.forward, out hit, 100f, playerMask))
        {
            Health health = hit.transform.GetComponent<Health>();

            if (health && health.hp > 0)
            {
                health.ApplyDamage(flameDamage, hit.transform.InverseTransformPoint(hit.point));
                if (health.hp <= 0)
                {
                    StartCoroutine(ParticleManager.Instance.BloodTraceEffect(hit.transform.position));
                }
                flameThrower.UseFlameThrower();

                StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));

            }
            else
            {
                if (!hit.transform.tag.Equals(playerStr))
                {
                    StartCoroutine(ParticleManager.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
                }
                else
                {
                    StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));
                }
            }
        }


        currentBullets--;
        fireTimer = 0.0f;
        anim.CrossFadeInFixedTime("Shoot", 0.01f);
        //audioSource.PlayOneShot(shootSound);    //shoot sound
                                                //muzzleFlash.Play();
    }

    public void Healing()
    {
        if (!isHealing)
            anim.SetBool(hashHeal, true);

    }

    public void DoReload()
    {
        if (!isReloading && currentBullets < bulletsPerMag && bulletsTotal > 0)
        {
            anim.CrossFadeInFixedTime("Reload", 0.01f); // Reloading
                                                        //audioSource.PlayOneShot(reloadSound);
        }
    }

    public void Reload()
    {
        int bulletsToReload = bulletsPerMag - currentBullets;
        if (bulletsToReload > bulletsTotal)
        {
            bulletsToReload = bulletsTotal;
        }
        currentBullets += bulletsToReload;
        bulletsTotal -= bulletsToReload;
    }

}
