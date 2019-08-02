using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfecteeGirlCtrl : MonoBehaviour
{
    public float recognizeRange;
    public Transform target;
    public SkinnedMeshRenderer mySkin;
    public bool isAttacked = false;

    private bool wasBoom = false;
    private AudioSource audiosrc;
    public AudioClip[] soundClips;
    public GameObject boomParticle;
    Health info;

    [HideInInspector]
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audiosrc = GetComponent<AudioSource>();
        info = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wasBoom)
            return;

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= recognizeRange || isAttacked)
        {
            anim.SetBool("isBoom", true);
            wasBoom = true;
            Invoke("ScreamSoundPlay", 1.8f);
            Invoke("Boom", 2.5f);
        }
    }

    public void ChangeSkinColor()
    {
        mySkin.material.color += new Color(0.01f, 0, 0);
    }

    public void SetAttackTrigger()
    {
        isAttacked = true;
    }

    void Boom()
    {
        if (gameObject.activeSelf)
        {
            Instantiate(boomParticle, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), transform.rotation);
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void ScreamSoundPlay()
    {
        if (gameObject.activeSelf)
        {
            audiosrc.clip = soundClips[0];
            audiosrc.Play();
        }
    }

    public void AfterDie()
    {
        if (gameObject.activeSelf)
            Boom();

    }
}
