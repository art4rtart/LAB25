using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCtrl : MonoBehaviour
{
    // Weapon Specification
    public string weaponName;
    public int bulletsPerMag;
    public int bulletsTotal;
    public static int currentBullets = 30;
    public float range;
    public float fireRate;
    public float accuracy;
    public int damage;

    // Parameters
    private float fireTimer;
    public bool isReloaded;
    private bool isReloading;
    private bool isRunning;

    // Sounds
    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip drawSound;

    //Recoil
    public Transform camRecoil;
    public Vector3 recoilKickback;
    public float recoilAmount;

    // References
    public Transform shootPoint;
    public Transform pickingPoint;
    private Animator anim;
    public ParticleSystem muzzleFlash;
    private CharacterController characterController;

    private int useWard = 0;

    // Pickup
    private bool isPick = false;
    RaycastHit pick;


    // her0in
    public int bulletsToReload;
	public ZombieScanner scanner;
	public Zemmer zemmer;
	public ItemManager itemManager;
	public bool useAdrenaline;
	public bool useMedicalKit;
	int specialItemIndex;
	public MissionScripts missionScripts;
	
    private void Start()
    {
        characterController = GetComponentInParent<CharacterController>();

        anim = GameObject.Find("Player").GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        isReloading = info.IsName("Reload");

		if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonUp(0))
        {
            if (!isPick)
            {
                if (currentBullets > 0)
                    Fire();
                else
                {
                    DoReload();
                }
            }
            else
            {
                Pickup();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
            DoReload();

        if (Input.GetKeyDown(KeyCode.T))
        {
            isPick = !isPick;
            if (!isPick)
            {
                if (pick.transform)
                {
                    pick.transform.SetParent(null);
                    pick.transform.GetComponent<Rigidbody>().useGravity = true;
                }
            }
        }

        //her0in -------------------------------------------------------------------

        if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			anim.SetBool("Ward", false);
			anim.SetBool("useHarter", false);
			anim.SetTrigger("default");
			specialItemIndex = 0;
		}

		else if (Input.GetKeyDown(KeyCode.Alpha2) && !useMedicalKit &&!useAdrenaline && itemManager.medicalKitCount > 0)
        {
			anim.SetBool("Ward", false);
			specialItemIndex = 0;
			useMedicalKit = true;
            anim.CrossFadeInFixedTime("Heal", 0.01f);
		}

		else if (Input.GetKeyDown(KeyCode.Alpha3) && !useMedicalKit && !useAdrenaline && itemManager.adrenalineCount > 0)
		{
			useAdrenaline = true;
			anim.CrossFadeInFixedTime("Adrenaline", 0.01f);
		}

		else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
			specialItemIndex++;

			if (specialItemIndex % 2 == 1)
			{
				anim.SetBool("useHarter", false);
				anim.SetBool("Ward", true);
			}

			else if (specialItemIndex % 2 == 0)
			{
				if (itemManager.hasHearter)
				{
					anim.SetBool("Ward", false);
					anim.SetTrigger("itemChange");
					anim.SetBool("useHarter", true);
				}

				else
					specialItemIndex--;
			}
		}

		else if (Input.GetKeyDown(KeyCode.J))
        {
            anim.CrossFadeInFixedTime("Jammer", 0.01f);

       
            // zemmer.UseZemmer();
        }

        if ( anim.GetBool("Ward") && Input.GetMouseButtonDown(1))
        {
			anim.SetTrigger("useWard");

			missionScripts.GetComponent<Animator>().SetTrigger("Finish");
			missionScripts.Type();

			scanner.ScanDistance = 0;
			scanner.scanning = true;

			anim.SetBool("Ward", false);
			anim.SetTrigger("default");
			specialItemIndex = 0;
		}

		// ----------------------------------------------------------------------
       

        RecoilBack();
        Run();
    }

    private void FixedUpdate()
    {
        if (fireTimer < fireRate)
            fireTimer += Time.fixedDeltaTime;
    }
    private void Run()
    {
        anim.SetBool("isRun", Input.GetKey(KeyCode.LeftShift));
        isRunning = characterController.velocity.sqrMagnitude > 99 ? true : false;
        //Debug.Log(characterController.velocity.sqrMagnitude);
        anim.SetFloat("Speed", characterController.velocity.sqrMagnitude);
    }

    public void Fire()
    {
        if (fireTimer < fireRate || isReloading || isRunning)
        {
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward + Random.onUnitSphere * accuracy, out hit, range))
        {
            Health health = hit.transform.GetComponent<Health>();

            if (health && health.hp > 0)
            {
                health.ApplyDamage(damage, hit.transform.InverseTransformPoint(hit.point));
				if (!hit.transform.CompareTag("Breakable"))
				{
					StartCoroutine(Particle.Instance.BloodEffect(hit.point));

				}

				else
					StartCoroutine(Particle.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
            }
            else
            {
                StartCoroutine(Particle.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
            }
        }
        currentBullets--;
        fireTimer = 0.0f;
        anim.CrossFadeInFixedTime("Shoot", 0.01f);
        audioSource.PlayOneShot(shootSound);    //shoot sound
        muzzleFlash.Play();
        Recoil();
        StartCoroutine(Particle.Instance.BulletEffect());
    }

    public void Pickup()
    {
        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out pick, range))
        {
            if (pick.transform.CompareTag("Moveable"))
            {
                Debug.Log(pick.transform.name);
                pick.transform.GetComponent<Rigidbody>().useGravity = false;
                pick.transform.SetParent(pickingPoint.transform);
            }
        }
    }
    private void Recoil()
    {
        Vector3 recoilVector = new Vector3(Random.Range(-recoilKickback.x, recoilKickback.x), recoilKickback.y, recoilKickback.z);
        Vector3 recoilCamVector = new Vector3(-recoilVector.y * 200f, recoilVector.x * 200f, 0);

        camRecoil.localRotation = Quaternion.Slerp(camRecoil.localRotation, Quaternion.Euler(camRecoil.localEulerAngles + recoilCamVector), recoilAmount); // cam recoil
    }

    // back to original position
    private void RecoilBack()
    {
        camRecoil.localRotation = Quaternion.Slerp(camRecoil.localRotation, Quaternion.identity, Time.deltaTime * 8f);
    }

    private void DoReload()
    {

        if (!isReloading && currentBullets < bulletsPerMag && bulletsTotal > 0)
        {
            anim.CrossFadeInFixedTime("Reload", 0.01f); // Reloading
            audioSource.PlayOneShot(reloadSound);
        }
    }

	public void Reload()
    {
		// her0in
		isReloaded = true;
		bulletsToReload = bulletsPerMag - currentBullets;
        if (bulletsToReload > bulletsTotal)
        {
            bulletsToReload = bulletsTotal;
        }
        bulletsTotal -= bulletsToReload;
    }

}

