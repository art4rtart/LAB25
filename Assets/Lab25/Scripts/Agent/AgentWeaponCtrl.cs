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

        //anim = GameObject.Find("Agent(ARMED)").GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (fireTimer < fireRate)
            fireTimer += Time.fixedDeltaTime;
		if (shotFireTimer < shotFireRate)
			shotFireTimer += Time.fixedDeltaTime;
		if (flameFireTimer < flameFireRate)
			flameFireTimer += Time.fixedDeltaTime;
		//Vector3 forward = transform.TransformDirection(Vector3.forward) * 199f;
		Debug.DrawRay(transform.position + Vector3.up, transform.forward * 300, Color.green);
	}

    public void Fire()
    {
		if (fireTimer < fireRate)
		{
			return;
		}
		RaycastHit hit;

        for (int i = 0; i < 1; ++i)
        {
			if (Physics.Raycast(shootPos.position, shootPos.transform.forward + Random.onUnitSphere * accuracy, out hit, range + 500f))
            {
                //Her0inEnemy enemyCtrl = hit.transform.GetComponent<Her0inEnemy>();
                //InfecteeGirlCtrl enemyGirlCtrl = hit.transform.GetComponent<InfecteeGirlCtrl>();
                Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
                Health health = hit.transform.GetComponent<Health>();

                // Debug.Log(hit.transform.gameObject.name);

                if (health && health.hp > 0)
                {
                    health.ApplyDamage(damage, hit.transform.InverseTransformPoint(hit.point));
                    if (labAgent.target.GetComponent<Feature>().nameHash != 4 && labAgent.target.GetComponent<Feature>().nameHash != 5 && labAgent.target.GetComponent<Feature>().nameHash != 1)
                    {
                        labAgent.AddReward(1f);
                        //Debug.Log("Good Shot Normal and Boss, I Used Normal Gun");
                    }
                    else
                    {
                        labAgent.AddReward(-5f);
                        //Debug.Log("Bad (Normal)");
                        labAgent.normalGunMissed++;
                    }

					if (!hit.transform.CompareTag("Breakable"))
					{
						//StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));
					}

					//else
						//StartCoroutine(ParticleManager.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
				}
                else
                {
					return;
					//StartCoroutine(ParticleManager.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
				}
            }
			else return;
		}
        currentBullets--;
        fireTimer = 0.0f;
        anim.CrossFadeInFixedTime("Shoot", 0.01f);
		audioSource.PlayOneShot(shootSound);    //shoot sound
		muzzleFlash.Play();
	}

    public void FireShotGun()
    {
        if (shotFireTimer < shotFireRate )
        {
            return;
        }

        RaycastHit hit;
        for (int i = 0; i < 5; ++i)
        {
            if (Physics.Raycast(shootPos.position, shootPos.transform.forward + Random.onUnitSphere * accuracy, out hit, range))
            {
                //Her0inEnemy enemyCtrl = hit.transform.GetComponent<Her0inEnemy>();
                //InfecteeGirlCtrl enemyGirlCtrl = hit.transform.GetComponent<InfecteeGirlCtrl>();
                //Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
                Health health = hit.transform.GetComponent<Health>();

                // Debug.Log(hit.transform.gameObject.name);

                if (health && health.hp > 0)
                {
                    health.ApplyDamage(shotGunDamage, hit.transform.InverseTransformPoint(hit.point));
                    if (labAgent.target.GetComponent<Feature>().nameHash == 5)
                    {
                        labAgent.AddReward(1f);
                        //Debug.Log("Good Shot Police, I Used Shot Gun");
					}
                    else
                    {
                        labAgent.AddReward(-5f);
                        //Debug.Log("Bad (ShotGun)");
                        labAgent.ShotGunMissed++;
                    }
					if (!hit.transform.CompareTag("Breakable"))
					{
						//StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));
					}
					else
						StartCoroutine(ParticleManager.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
				}
				else
				{
					//Debug.Log("Amazing");
					//StartCoroutine(ParticleManager.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
				}
            }
        }
        currentBullets--;
        shotFireTimer = 0.0f;

        anim.CrossFadeInFixedTime("Shoot", 0.01f);
		audioSource.PlayOneShot(shotGunSound);    //shoot sound
		muzzleFlash.Play();
	}

    public void FireFlameThrower()
    {
		if (flameFireTimer < flameFireRate)
		{
			return;
		}

		RaycastHit hit;
        for (int i = 0; i < 1; ++i)
        {
			if (Physics.Raycast(shootPos.position, shootPos.transform.forward, out hit, 7f))
			{
				//Her0inEnemy enemyCtrl = hit.transform.GetComponent<Her0inEnemy>();
				//InfecteeGirlCtrl enemyGirlCtrl = hit.transform.GetComponent<InfecteeGirlCtrl>();
				Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
				Health health = hit.transform.GetComponent<Health>();

				// Debug.Log(hit.transform.gameObject.name);

				if (health && health.hp > 0)
				{
					health.ApplyDamage(flameDamage, hit.transform.InverseTransformPoint(hit.point));
					if (labAgent.target.GetComponent<Feature>().nameHash == 4)
					{
						labAgent.AddReward(1f);
						//Debug.Log("Good Shot Transparent, I Used Flame Thrower");
						flameThrower.UseFlameThrower();
					}
					else
					{
						labAgent.AddReward(-5f);
						//Debug.Log("Bad (Flame)");
						labAgent.flameThrowerMissed++;
					}

					//if(health.hp <= 0)
					//{
					//	Debug.Log("Stop!!!");
					//	flameThrower.StopFlameThrower();
					//}
					//if (!hit.transform.CompareTag("Breakable"))
					//{
					//	StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));
					//}
					//else
					//	StartCoroutine(ParticleManager.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
				}
				else
				{
					return;
					//StartCoroutine(ParticleManager.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
				}
			}
			else
			{
				return;
			}
		}
        currentBullets--;
        flameFireTimer = 0.0f;
        anim.CrossFadeInFixedTime("Shoot", 0.01f);
		audioSource.PlayOneShot(shootSound);    //shoot sound
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

	float sight = 20f;
	void OnDrawGizmosSelected()
	{
		Vector3 forward = transform.TransformDirection(Vector3.forward) * sight;
		Debug.DrawRay(transform.position, forward, Color.green);
	}
}
