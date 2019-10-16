using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCtrl : MonoBehaviour
{
    public static WeaponCtrl Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<WeaponCtrl>();
            return instance;
        }
    }

    private static WeaponCtrl instance;

    // Weapon SpecificationcurrentBullets
    // AK
    public int akBulletsPerMag = 30;
	public int akBulletsTotal = 360;
	[HideInInspector] public int akCurrentBullets = 50;
    private float akRange = 100f;
    public float akFireRate = 0.1f;
    private float akAccuracy = 0.015f;
    private int akDamage = 43;

    // Scifi
    private int sciBulletsPerMag = 150;
    [HideInInspector]public int sciBulletsTotal = 1500;
    [HideInInspector]public int sciCurrentBullets = 150;
    private float sciRange = 100f;
    private float sciFireRate = 0.05f;
    private float sciAccuracy = 0.005f;
    private int sciDamage = 50;

    // Axe
    private float AxeRate = 0.25f;

    // Bong
    private float BpngRate = 0.35f;

    // Parameters
    private float fireTimer;
    private bool isReloading;
    private bool isRunning;
    private float AxeTimer;
    private float BongTimer;
    //[HideInInspector]public bool hasBong = false;

    // Sounds
    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip axeSound;
    public AudioClip itemGetSound;
    public AudioClip reloadSound;
    public AudioClip drawSound;
    public AudioClip heartbeatSound;

    //Recoil
    public Transform camRecoil;
    public Vector3 recoilKickback;
    public float recoilAmount;

    // References
    public Transform shootPoint;
    public Transform pickingPoint;
    [HideInInspector] public Animator anim;
    public ParticleSystem muzzleFlash;
    private CharacterController characterController;
    private int useWard = 0;


    // her0in
    public int bulletsToReload;
    public ZombieScanner scanner;
    public Zemmer zemmer;
    public UIManager uiManager;
    public ItemManager itemManager;
    public bool useAdrenaline;
    public bool useMedicalKit;
    int specialItemIndex;
    public MissionScripts missionScripts;

    public enum WEAPON { AKM, SCI_FI, AXE, CUP, BOMB, HEARTER, WARD, JAMMER, ADRE, HEAL, BONG };
    public WEAPON myWeapnType;

    public int stage;
    public GrenadeThrower grenadeThrower;

    public string reloadStr = "SciFiReload";
	public bool isUTRSMode;
    private int playerMask;
    private readonly string playerStr = "Player";
    // Heartbeat
    private float heartbeatFreq = 0.65f;
    private void Awake()
    {
        characterController = GetComponentInParent<CharacterController>();
        anim = GameObject.Find("Player").GetComponent<Animator>();

		akCurrentBullets = akBulletsPerMag;
		UIManager.Instance.bulletsPerMagazine = akCurrentBullets;
		UIManager.Instance.totalBullet = akBulletsTotal;

        playerMask = (-1) - (1 << LayerMask.NameToLayer("Player")) + (1 << LayerMask.NameToLayer("Item")); ;
    }

	public bool isHoldingBomb;

	private void Update()
	{
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
		isReloading = info.IsName(reloadStr);

		//if (info.IsName("EndToDo(AK)") || info.IsName("Idle(AK)"))
		//    myWeapnType = WEAPON.AKM;

		if (myWeapnType != WEAPON.BOMB) isHoldingBomb = false;
		else isHoldingBomb = true;

		//Debug.Log(myWeapnType);
		if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonUp(0))
		{
			if (myWeapnType.Equals(WEAPON.AKM))
			{
				if (akCurrentBullets > 0)
					Fire();
				else
					DoReload();
			}
			else if (myWeapnType.Equals(WEAPON.SCI_FI))
			{
				if (sciCurrentBullets > 0)
					Scifi_Fire();
				else
					DoReload();
			}
			else if (myWeapnType.Equals(WEAPON.AXE))
			{
				AxeAttack();
			}
			else if (myWeapnType.Equals(WEAPON.BONG))
			{
				BongAttack();
			}
		}
		else if (Input.GetMouseButtonDown(0))
		{
			 if (myWeapnType.Equals(WEAPON.CUP) /*&& itemManager.beakerCount > 0*/)
			{
	
				grenadeThrower.ThrowGrenade();
				itemManager.beakerCount--;
				grenadeThrower.lineRenderer.enabled = false;
				//StartCoroutine(grenadeThrower.ThrowCup());
				grenadeThrower.alpha = 0;
				//itemManager.readyToUseGrenade = false;
				//Anim End             
			}
			else if (myWeapnType.Equals(WEAPON.BOMB))
			{
				if (BombGage.Instance.canInstall && !BombGage.Instance.isInstalling)
				{
					StartCoroutine(BombGage.Instance.BombInstall());
					BombGage.Instance.isInstalling = true;
				}
			}

			else if (myWeapnType.Equals(WEAPON.HEARTER))
			{

			}

			else if (myWeapnType.Equals(WEAPON.WARD))
			{
				anim.SetTrigger("useWard");
				scanner.scanning = true;

				StartCoroutine("DelayResetuseWard");
			}

			else if (myWeapnType.Equals(WEAPON.JAMMER))
			{

			}
			else if (myWeapnType.Equals(WEAPON.ADRE))
			{

			}
			else if (myWeapnType.Equals(WEAPON.HEAL))
			{

			}
		}
     
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
				//Debug.Log("Click");
				if (info.IsName("Idle(AXE)") || info.IsName("Idle(Iron)"))
				{
					anim.SetTrigger("doWeaponChange");

				}
				else
				{
					//Debug.Log("Click Ing");


					anim.SetTrigger("endHearter");
					anim.SetTrigger("endBomb");
					anim.SetBool("doThrow", false);

				}

                StartCoroutine("DelayResetdoWeaponChange");

            }
			else if (Input.GetKeyDown(KeyCode.R))
			{
				if (myWeapnType.Equals(WEAPON.AKM) || myWeapnType.Equals(WEAPON.SCI_FI))
				{
					DoReload();
				}
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3) && itemManager.medicalKitCount > 0)
            {
				// Heal
				useMedicalKit = true;
                anim.SetBool("toDo", true);
                anim.SetTrigger("doHeal");
                StartCoroutine("DelayResetAnimParameter");
            }

            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // To Axe or To Bong
                if ( info.IsName("Idle(AK)") || info.IsName("Idle(Scifi)") )
                {
                    anim.SetTrigger("doWeaponChange");
                    StartCoroutine("DelayResetdoWeaponChange");
                }	
				else
				{
					anim.SetTrigger("endHearter");
					anim.SetTrigger("endBomb");
					anim.SetBool("doThrow", false);
				}
			}
            else if (Input.GetKeyDown(KeyCode.C))
            {
                // To Throw Cup
                anim.SetBool("toDo", true);
				anim.SetBool("doThrow", true);
				grenadeThrower.lineRenderer.enabled = true;

                StartCoroutine("DelayResetAnimParameter");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6) && itemManager.hasWard)
            {
                // Ward
                anim.SetBool("toDo", true);
                anim.SetTrigger("doWard");

                StartCoroutine("DelayResetAnimParameter");
            }

            else if (Input.GetKeyDown(KeyCode.Alpha5) && itemManager.hasHearter)
            {
                // Hearter
                anim.SetBool("toDo", true);
                anim.SetTrigger("doHearter");

                StartCoroutine("DelayResetAnimParameter");
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                // Bomb
                anim.SetBool("toDo", true);
                anim.SetTrigger("doBomb");

                StartCoroutine("DelayResetAnimParameter");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                // Jammer
                anim.SetBool("toDo", true);
                anim.SetTrigger("doJammer");

                StartCoroutine("DelayResetAnimParameter");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && itemManager.adrenalineCount > 0)
            {
                // Adre
                StartCoroutine("HeartbeatSoundPlay");
				useAdrenaline = true;
                anim.SetBool("toDo", true);
                anim.SetTrigger("doAdre");

                StartCoroutine("DelayResetAnimParameter");
            }
        }

        RecoilBack();
        Run();
    }

    private void FixedUpdate()
    {
        //Debug.DrawRay(shootPoint.transform.position, (shootPoint.transform.forward * 2 + shootPoint.transform.right).normalized * 100f, Color.red);
        //Debug.DrawRay(shootPoint.transform.position, ((shootPoint.transform.forward * 2 + shootPoint.transform.right * -1f)).normalized * 100f, Color.red);

        if (myWeapnType .Equals( WEAPON.AKM))
        {
            if (fireTimer < akFireRate)
                fireTimer += Time.fixedDeltaTime;
        }
        else if (myWeapnType .Equals( WEAPON.SCI_FI))
        {
            if (fireTimer < sciFireRate)
                fireTimer += Time.fixedDeltaTime;
        }
        if (AxeTimer < AxeRate)
            AxeTimer += Time.fixedDeltaTime;

        if (BongTimer < 0.3f)
            BongTimer += Time.fixedDeltaTime;
    }
    private void AxeAttack()
    {
        if (AxeTimer < AxeRate)
        {
            return;
        }
        AxeTimer = 0.0f;
        audioSource.PlayOneShot(axeSound);
        //anim.CrossFadeInFixedTime("Shoot", 0.01f);
        //audioSource.PlayOneShot(shootSound);    //shoot sound
        anim.CrossFadeInFixedTime("AxeAttack", 0.01f);
    }

    private void BongAttack()
    {
        if (BongTimer < 0.3f)
        {
            return;
        }
     
        //    else
        //    {
        //        StartCoroutine(ParticleManager.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
        //    }
        //}
        BongTimer = 0.0f;
        audioSource.PlayOneShot(axeSound);
        //anim.CrossFadeInFixedTime("Shoot", 0.01f);
        //audioSource.PlayOneShot(shootSound);    //shoot sound
        anim.CrossFadeInFixedTime("BongAttack", 0.01f);
    }

    public void DamagedByAxe()
    {
        RaycastHit hit, hit2, hit3;

        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, 3f, playerMask))
        {
            Health health = hit.transform.GetComponent<Health>();

            if (health && health.hp > 0)
            {
                health.ApplyDamage(300, hit.transform.InverseTransformPoint(hit.point));
                if (health.hp <= 0)
                {
                    StartCoroutine(ParticleManager.Instance.BloodTraceEffect(hit.transform.position));
                }

                StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));


                //else
                //    StartCoroutine(ParticleManager.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
            }
            else
            {
                if (transform.tag.Equals(playerStr))
                    StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));
            }
        }
    }

    public void DamagedByBong()
    {
        RaycastHit hit, hit2, hit3;

        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, 3f, playerMask))
        {
            Health health = hit.transform.GetComponent<Health>();

            if (health && health.hp > 0)
            {
                hit.transform.GetComponentInParent<ChangeRagDoll>().attackedByElectricStick = true;
                health.ApplyDamage(300, hit.transform.InverseTransformPoint(hit.point));
                if (health.hp <= 0)
                {
                    StartCoroutine(ParticleManager.Instance.BloodTraceEffect(hit.transform.position));
                }

                StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));


            }
            else
            {
                if (transform.tag.Equals(playerStr))
                    StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));
            }
        }

        if (Physics.Raycast(shootPoint.position, (shootPoint.transform.forward * 2 + shootPoint.transform.right).normalized, out hit2, 3f, playerMask))
        {
            Health health = hit2.transform.GetComponent<Health>();

            if (health && health.hp > 0)
            {
                hit2.transform.GetComponentInParent<ChangeRagDoll>().attackedByElectricStick = true;
                health.ApplyDamage(300, hit2.transform.InverseTransformPoint(hit2.point));
                if (health.hp <= 0)
                {
                    StartCoroutine(ParticleManager.Instance.BloodTraceEffect(hit2.transform.position));
                }

                StartCoroutine(ParticleManager.Instance.BloodEffect(hit2.point));


            }
            else
            {
                if (transform.tag.Equals(playerStr))
                    StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));
            }
        }

        if (Physics.Raycast(shootPoint.position, (shootPoint.transform.forward * 2 + shootPoint.transform.right * -1f).normalized, out hit3, 3f, playerMask))
        {
            Health health = hit3.transform.GetComponent<Health>();

            if (health && health.hp > 0)
            {
                hit3.transform.GetComponentInParent<ChangeRagDoll>().attackedByElectricStick = true;
                health.ApplyDamage(300, hit3.transform.InverseTransformPoint(hit3.point));
                if (health.hp <= 0)
                {
                    StartCoroutine(ParticleManager.Instance.BloodTraceEffect(hit3.transform.position));
                }

                StartCoroutine(ParticleManager.Instance.BloodEffect(hit3.point));


            }
            else
            {
                if (transform.tag.Equals(playerStr))
                    StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));
            }
        }
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
        if (fireTimer < akFireRate || isReloading || isRunning)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward + Random.onUnitSphere * akAccuracy, out hit, akRange, playerMask))
        {
			//Debug.Log(hit.transform.name);
			Health health = hit.transform.GetComponent<Health>();

			if (health && health.hp > 0)
			{
				health.ApplyDamage(akDamage, hit.transform.InverseTransformPoint(hit.point));
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
        akCurrentBullets--;
        fireTimer = 0.0f;
        anim.CrossFadeInFixedTime("Shoot(AK)", 0.01f);
        audioSource.PlayOneShot(shootSound);    //shoot sound
        muzzleFlash.Play();
        Recoil();
        StartCoroutine(ParticleManager.Instance.BulletEffect());
        UIManager.Instance.TextUpdate();
    }

    public void Scifi_Fire()
    {
        if (fireTimer < sciFireRate || isReloading || isRunning)
        {
            return;
        }

        RaycastHit hit;
  
        for (int i = 0; i < 3; ++i)
        {
            if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward + Random.onUnitSphere * sciAccuracy, out hit, sciRange, playerMask))
            {
                Health health = hit.transform.GetComponent<Health>();

                if (health && health.hp > 0)
                {
                    health.ApplyDamage(sciDamage, hit.transform.InverseTransformPoint(hit.point));
                    if (health.hp <= 0)
                    {
                        StartCoroutine(ParticleManager.Instance.BloodTraceEffect(hit.transform.position));
                    }

                    StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));

                }
                else
                {
                    if( !hit.transform.tag.Equals(playerStr))
                        StartCoroutine(ParticleManager.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
                    else
                        StartCoroutine(ParticleManager.Instance.BloodEffect(hit.point));
                }
            }
        }
        sciCurrentBullets--;
        fireTimer = 0.0f;
        anim.CrossFadeInFixedTime("Shoot(Scifi)", 0.01f);
        audioSource.PlayOneShot(shootSound);    //shoot sound
        muzzleFlash.Play();
        Recoil();
        StartCoroutine(ParticleManager.Instance.BulletEffect());
        UIManager.Instance.TextUpdate();
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

    public void DamageRecoil()
    {
        //Vector3 recoilVector = new Vector3(Random.Range(-0.25f, 0.25f), -200, 0);
        Vector3 recoilCamVector = new Vector3(-200, Random.Range(-0.25f, 0.25f) * 200f, 0);

        //transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + recoilVector, recoilAmount / 2f); // position recoil
        camRecoil.localRotation = Quaternion.Slerp(camRecoil.localRotation, Quaternion.Euler(camRecoil.localEulerAngles + recoilCamVector), recoilAmount); // cam recoil
    }

    private void DoReload()
    {
        if (myWeapnType.Equals( WEAPON.AKM))
        {
			if (isUTRSMode) reloadStr = "AKReload";
            if (!isReloading && akCurrentBullets < akBulletsPerMag && akBulletsTotal > 0)
            {
                anim.CrossFadeInFixedTime("AKReload", 0.01f); // Reloading
                audioSource.PlayOneShot(reloadSound);
              
            }
        }
        else if (myWeapnType.Equals( WEAPON.SCI_FI))
        {
			if (isUTRSMode) reloadStr = "SciFiReload";

			if (!isReloading && sciCurrentBullets < sciBulletsPerMag && sciBulletsTotal > 0)
            {
                anim.CrossFadeInFixedTime("SciFiReload", 0.01f); // Reloading
                audioSource.PlayOneShot(reloadSound);
               
            }
        }
    }

    public void Reload()
    {
        // her0in
        if (myWeapnType .Equals( WEAPON.AKM))
        {
			if (isUTRSMode) reloadStr = "AKReload";
			bulletsToReload = akBulletsPerMag - akCurrentBullets;
            if (bulletsToReload > akBulletsTotal)
            {
                bulletsToReload = akBulletsTotal;
            }
            akBulletsTotal -= bulletsToReload;
        }
        else if (myWeapnType .Equals( WEAPON.SCI_FI))
        {
			if (isUTRSMode) reloadStr = "SciFiReload";
			bulletsToReload = sciBulletsPerMag - sciCurrentBullets;
            if (bulletsToReload > sciBulletsTotal)
            {
                bulletsToReload = sciBulletsTotal;
            }
            sciBulletsTotal -= bulletsToReload;
        }
    }

	public void ResetParameterDoThrow()
	{
		anim.SetBool("doThrow", false);
	}
    private IEnumerator DelayResetAnimParameter()
    {
        yield return new WaitForSeconds(1.5f);
        anim.SetBool("toDo", false);
    }

    private IEnumerator DelayResetuseWard()
    {
        yield return new WaitForSeconds(0.5f);
        anim.ResetTrigger("useWard");
    }

    private IEnumerator DelayResetdoWeaponChange()
    {
        //Debug.Log("ASDSAD");
        yield return new WaitForSeconds(0.8f);

        anim.ResetTrigger("doWeaponChange");
        anim.ResetTrigger("endHearter");
        anim.ResetTrigger("endBomb");
        //Debug.Log("ASD");

    }

    private IEnumerator HeartbeatSoundPlay()
    {
        for( int i = 0; i < 10; ++i )
        {
            yield return new WaitForSeconds(heartbeatFreq);
            if (i == 4)
                heartbeatFreq = 0.4f;
            audioSource.clip = heartbeatSound;
            audioSource.Play();
        }
        heartbeatFreq = 0.65f;
    }

}

