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
    public string weaponName;
    public int bulletsPerMag;
    public int bulletsTotal;
    public int currentBullets;
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
    public AudioClip axeSound;
    public AudioClip itemGetSound;
    public AudioClip reloadSound;
    public AudioClip drawSound;

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

    public enum WEAPON { AKM, SCI_FI, AXE, CUP, PICK, BOMB, HEARTER, WARD, JAMMER, ADRE, HEAL };
    public WEAPON myWeapnType;

    public int stage;
    public GrenadeThrower grenadeThrower;

    private void Start()
    {
        characterController = GetComponentInParent<CharacterController>();
        anim = GameObject.Find("Player").GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        isReloading = info.IsName("Reload");

        //if (info.IsName("EndToDo(AK)") || info.IsName("Idle(AK)"))
        //    myWeapnType = WEAPON.AKM;

        //Debug.Log(myWeapnType);
        if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonUp(0))
        {
            if (myWeapnType == WEAPON.AKM)
            {
                if (currentBullets > 0)
                    Fire();
                else
                    DoReload();
            }
            else if (myWeapnType == WEAPON.SCI_FI)
            {
                if (currentBullets > 0)
                    Scifi_Fire();
                else
                    DoReload();
            }
            else if (myWeapnType == WEAPON.AXE)
            {
                AxeAttack();
            }
            else if (myWeapnType == WEAPON.CUP)
            {
                anim.SetTrigger("doThrow");
                grenadeThrower.ThrowGrenade();
                //StartCoroutine(grenadeThrower.ThrowCup());
                grenadeThrower.alpha = 0;
                //itemManager.readyToUseGrenade = false;
                //Anim End             
            }
            else if (myWeapnType == WEAPON.PICK)
            {

            }
            else if (myWeapnType == WEAPON.BOMB)
            {

            }
            else if (myWeapnType == WEAPON.HEARTER)
            {

            }
            else if (myWeapnType == WEAPON.WARD)
            {
                anim.SetTrigger("useWard");

                StartCoroutine("DelayResetuseWard");
            }
            else if (myWeapnType == WEAPON.JAMMER)
            {

            }
            else if (myWeapnType == WEAPON.ADRE)
            {

            }
            else if (myWeapnType == WEAPON.HEAL)
            {

            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (myWeapnType == WEAPON.AKM || myWeapnType == WEAPON.SCI_FI)
            {
                DoReload();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (info.IsName("Idle(AXE)"))
                {
                    anim.SetTrigger("doWeaponChange");

                }
                else
                {
                    anim.SetTrigger("endHearter");
                    anim.SetTrigger("endBomb");
                }
                //// To Main Weapon
                //if (stage == 3)
                //    myWeapnType = WEAPON.AKM;
                //else if (stage == 4 || stage == 5)
                //    myWeapnType = WEAPON.SCI_FI;
                StartCoroutine("DelayResetdoWeaponChange");

            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // Heal
                anim.SetBool("toDo", true);
                anim.SetTrigger("doHeal");
                StartCoroutine("DelayResetAnimParameter");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                // To Axe
                if (info.IsName("Idle(AK)") || info.IsName("Run(AK)") || info.IsName("Idle(SCIFI)") || info.IsName("Run(SCIFI)"))
                {
                    anim.SetTrigger("doWeaponChange");
                    StartCoroutine("DelayResetdoWeaponChange");
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                // To Throw Cup
                anim.SetBool("toDo", true);
                grenadeThrower.lineRenderer.enabled = true;

                StartCoroutine("DelayResetAnimParameter");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                // Ward
                anim.SetBool("toDo", true);
                anim.SetTrigger("doWard");

                StartCoroutine("DelayResetAnimParameter");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
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
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                // Adre
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
        if (fireTimer < fireRate)
            fireTimer += Time.fixedDeltaTime;
        if (AxeTimer < AxeRate)
            AxeTimer += Time.fixedDeltaTime;

    }
    private void AxeAttack()
    {
        if (AxeTimer < AxeRate)
        {
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, 1.5f))
        {
            Health health = hit.transform.GetComponent<Health>();

            if (health && health.hp > 0)
            {
                health.ApplyDamage(damage * 5, hit.transform.InverseTransformPoint(hit.point));
                if (!hit.transform.CompareTag("Breakable"))
                {
                    StartCoroutine(Particle.Instance.BloodEffect(hit.point));

                }

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
        audioSource.PlayOneShot(axeSound);
        //anim.CrossFadeInFixedTime("Shoot", 0.01f);
        //audioSource.PlayOneShot(shootSound);    //shoot sound
        anim.CrossFadeInFixedTime("AxeAttack", 0.01f);
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
        anim.CrossFadeInFixedTime("Shoot(AK)", 0.01f);
        audioSource.PlayOneShot(shootSound);    //shoot sound
        muzzleFlash.Play();
        Recoil();
        StartCoroutine(Particle.Instance.BulletEffect());
        UIManager.Instance.TextUpdate();
    }

    public void Scifi_Fire()
    {
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
                anim.CrossFadeInFixedTime("Reload(AK)", 0.01f); // Reloading
                audioSource.PlayOneShot(reloadSound);
            }
        }
        else if (myWeapnType == WEAPON.SCI_FI)
        {
            if (!isReloading && currentBullets < bulletsPerMag && bulletsTotal > 0)
            {
                anim.CrossFadeInFixedTime("Reload(Scifi)", 0.01f); // Reloading
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
    private IEnumerator DelayResetAnimParameter()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("toDo", false);
    }

    private IEnumerator DelayResetuseWard()
    {
        yield return new WaitForSeconds(0.5f);
        anim.ResetTrigger("useWard");
    }

    private IEnumerator DelayResetdoThrow()
    {
        yield return new WaitForSeconds(0.5f);
        anim.ResetTrigger("doThrow");
    }

    private IEnumerator DelayResetdoWeaponChange()
    {
        //Debug.Log("ASDSAD");
        yield return new WaitForSeconds(0.5f);

        anim.ResetTrigger("doWeaponChange");
        anim.ResetTrigger("endHearter");
        anim.ResetTrigger("endBomb");
        //Debug.Log("ASD");

    }
}

