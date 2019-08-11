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

    // Axe
    private float AxeRate = 0.25f;
    // Parameters
    private float fireTimer;
    private bool isReloading;
    private bool isRunning;
    private float AxeTimer;

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
	public UIManager uiManager;
    public ItemManager itemManager;
    public bool useAdrenaline;
    public bool useMedicalKit;
    int specialItemIndex;
    public MissionScripts missionScripts;

    public enum WEAPON { AKM, SCI_FI, AXE, CUP, PICK, BOMB };
    public WEAPON myWeapnType;

    void Awake()
    {
        bulletsTotal = 360;
        currentBullets = 30;
    }

    private void Start()
    {
        characterController = GetComponentInParent<CharacterController>();
        anim = GameObject.Find("Player").GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
       

        if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonUp(0))
        {
            if (myWeapnType == WEAPON.AKM)
            {
                isReloading = info.IsName("Reload");
                if (currentBullets > 0)
                    Fire();
                else
                    DoReload();
            }
            else if (myWeapnType == WEAPON.AXE)
            {
                AxeAttack();
            }
            else if (myWeapnType == WEAPON.CUP)
            {

            }
            else if (myWeapnType == WEAPON.SCI_FI)
            {
                isReloading = info.IsName("Reload");
                if (currentBullets > 0)
                    Scifi_Fire();
                else
                    DoReload();
            }
            else if (myWeapnType == WEAPON.BOMB)
            {

            }
            else if (myWeapnType == WEAPON.PICK)
            {
                Pickup();
            }
        }
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    isPick = !isPick;
        //    if (myWeapnType != WEAPON.PICK)
        //    {
        //        if (pick.transform)
        //        {
        //            pick.transform.SetParent(null);
        //            pick.transform.GetComponent<Rigidbody>().useGravity = true;
        //        }
        //    }
        //}
        if (myWeapnType == WEAPON.AKM )
        {
            if (Input.GetKeyDown(KeyCode.R))
                DoReload();
            //her0in -------------------------------------------------------------------

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                anim.SetBool("Ward", false);
                anim.SetBool("useHarter", false);
                anim.SetTrigger("default");
                specialItemIndex = 0;
				uiManager.currentWeaponImage.sprite = uiManager.weaponImage[0];
				uiManager.TextUpdate();
			}

            else if (Input.GetKeyDown(KeyCode.Alpha2) && !useMedicalKit && !useAdrenaline && itemManager.medicalKitCount > 0)
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
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (info.IsName("Idle"))
                {
                    anim.SetTrigger("toAxe");
                    myWeapnType = WEAPON.AXE;
					uiManager.currentWeaponImage.sprite = uiManager.weaponImage[2];
					uiManager.bulletCountText.text = "1";
					uiManager.totalBulletText.text = "1";
				}
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                if (info.IsName("Idle"))
                {
                    anim.SetBool("toScifi", true);
                    myWeapnType = WEAPON.SCI_FI;
                }
            }
            if (anim.GetBool("Ward") && Input.GetMouseButtonDown(1))
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
            //RecoilBack();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (info.IsName("IDLE(axe)"))
                {
                    anim.SetTrigger("toAK");
                    myWeapnType = WEAPON.AKM;
					uiManager.currentWeaponImage.sprite = uiManager.weaponImage[0];
					uiManager.TextUpdate();
				}

                else if(info.IsName("Walk(scifi)"))
                {
                    anim.SetBool("toScifi", false);
                    myWeapnType = WEAPON.AKM;
				}
            }
            else if( Input.GetKeyDown(KeyCode.R))
            {
                if (myWeapnType == WEAPON.SCI_FI)
                    DoReload();
            }
            //else if (Input.GetKeyDown(KeyCode.Alpha5))
            //{
                
            //    if (info.IsName("Idle"))
            //    {
            //        anim.SetTrigger("toAxe");
            //        myWeapnType = WEAPON.AXE;
            //    }
            //}
        }

        // ----------------------------------------------------------------------
        RecoilBack();
        Run();
    }

    private void FixedUpdate()
    {
        if (fireTimer < fireRate)
            fireTimer += Time.fixedDeltaTime;
        if (AxeTimer < AxeRate)
            AxeTimer += Time.fixedDeltaTime;

    }
    private void AxeAttack()
    {
        if (AxeTimer < AxeRate )
        {
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward , out hit, 1.5f))
        {
            Health health = hit.transform.GetComponent<Health>();

            if (health && health.hp > 0)
            {
                health.ApplyDamage(damage * 5, hit.transform.InverseTransformPoint(hit.point));
                //if (!hit.transform.CompareTag("Breakable"))
                //{
                //    StartCoroutine(Particle.Instance.BloodEffect(hit.point));

                //}

                //else
                //    StartCoroutine(Particle.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
            }
        }
        //    else
        //    {
        //        StartCoroutine(Particle.Instance.FireEffect(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)));
        //    }
        //}
        AxeTimer = 0.0f;
        //anim.CrossFadeInFixedTime("Shoot", 0.01f);
        //audioSource.PlayOneShot(shootSound);    //shoot sound
        anim.CrossFadeInFixedTime("attackAxe", 0.01f);
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
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        isReloading = info.IsName("Reload");
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
        UIManager.Instance.TextUpdate();
    }

    public void Scifi_Fire()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        isReloading = info.IsName("Reload");
        if (fireTimer < fireRate * 0.5f || isReloading || isRunning)
        {
            return;
        }

        RaycastHit hit;

        for (int i = 0; i < 3; ++i)
        {
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
        }
        currentBullets--;
        fireTimer = 0.0f;
        anim.CrossFadeInFixedTime("Shoot(scifi)", 0.01f);
        audioSource.PlayOneShot(shootSound);    //shoot sound
        muzzleFlash.Play();
        Recoil();
        StartCoroutine(Particle.Instance.BulletEffect());
        UIManager.Instance.TextUpdate();
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

    public void DamageRecoil()
    {
        //Vector3 recoilVector = new Vector3(Random.Range(-0.25f, 0.25f), -200, 0);
        Vector3 recoilCamVector = new Vector3(-200, Random.Range(-0.25f, 0.25f) * 200f, 0);

        //transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + recoilVector, recoilAmount / 2f); // position recoil
        camRecoil.localRotation = Quaternion.Slerp(camRecoil.localRotation, Quaternion.Euler(camRecoil.localEulerAngles + recoilCamVector), recoilAmount); // cam recoil
    }

    private void DoReload()
    {
        if (myWeapnType == WEAPON.AKM)
        {
            if (!isReloading && currentBullets < bulletsPerMag && bulletsTotal > 0)
            {
                anim.CrossFadeInFixedTime("Reload", 0.01f); // Reloading
                audioSource.PlayOneShot(reloadSound);
            }
        }
        else if (myWeapnType == WEAPON.SCI_FI)
        {
            if (!isReloading && currentBullets < bulletsPerMag && bulletsTotal > 0)
            {
                anim.CrossFadeInFixedTime("Reload(scifi)", 0.01f); // Reloading
                audioSource.PlayOneShot(reloadSound);
            }
        }
    }

    public void Reload()
    {
        // her0in
        bulletsToReload = bulletsPerMag - currentBullets;
        if (bulletsToReload > bulletsTotal)
        {
            bulletsToReload = bulletsTotal;
        }
        bulletsTotal -= bulletsToReload;
    }
}

